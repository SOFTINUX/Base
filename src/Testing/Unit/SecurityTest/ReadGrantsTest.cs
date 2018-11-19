using System.Collections.Generic;
using System.Linq;
using CommonTest;
using ExtCore.Infrastructure;
using SoftinuxBase.SeedDatabase;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.Security.ViewModels.Permissions;
using Xunit;
using Permission = SoftinuxBase.Security.Common.Enums.Permission;

namespace SecurityTest
{
    /// <summary>
    /// Note: the extensions we want to work with must be added to this project's references so that the extension is found
    /// at runtime in unit test working directory (using ExtCore's ExtensionManager and custom path).
    /// So we did for the Chinook extension. Security was already referenced.
    /// </summary>
    [Collection("Database collection")]
    public class ReadGrantsTest : CommonTestWithDatabase
    {
        public ReadGrantsTest(DatabaseFixture databaseFixture_) : base(databaseFixture_)
        {
        }

        /// <summary>
        /// Create base permissions, roles, one additional role "Special User".
        /// Then create role-extension links as following:
        ///
        /// "Security" extension:
        ///
        /// Administrator role : Admin permission
        /// User role : Read permission
        /// Anonymous role : Never permission
        /// Special User role : Write permission
        ///
        /// "Chinook" extension:
        ///
        /// Administrator role : Admin permission
        /// User role : Write permission
        ///
        /// </summary>
        [Fact]
        public async void Test()
        {
            var repo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();

            try
            {
                // Arrange
                // 1. Create base roles
                await CreateBaseRolesIfNeeded();

                // 2. Create "Special User" role
                await CreateRoleIfNotExisting("Special User");

                // 3. Read roles to get their IDs
                var adminRole = await DatabaseFixture.RoleManager.FindByNameAsync(Role.Administrator.GetRoleName());
                var userRole = await DatabaseFixture.RoleManager.FindByNameAsync(Role.User.GetRoleName());
                var anonymousRole = await DatabaseFixture.RoleManager.FindByNameAsync(Role.Anonymous.GetRoleName());
                var specialUserRole = await DatabaseFixture.RoleManager.FindByNameAsync("Special User");

                // 4. Create role-extension links
                // Cleanup first
                repo.DeleteAll();
                DatabaseFixture.Storage.Save();

                repo.Create(new RolePermission
                    { RoleId = adminRole.Id, Extension = "SoftinuxBase.Security", Name = Permission.Admin.GetPermissionName()});
                repo.Create(new RolePermission
                    {RoleId = userRole.Id, Extension = "SoftinuxBase.Security", Name = Permission.Read.GetPermissionName()});
                repo.Create(new RolePermission
                {
                    RoleId = anonymousRole.Id, Extension = "SoftinuxBase.Security", Name = Permission.Never.GetPermissionName()
                });
                repo.Create(new RolePermission
                {
                    RoleId = specialUserRole.Id, Extension = "SoftinuxBase.Security", Name = Permission.Write.GetPermissionName()
                });

                repo.Create(new RolePermission
                    {RoleId = adminRole.Id, Extension = "Chinook", Name = Permission.Admin.GetPermissionName()});
                repo.Create(new RolePermission
                    {RoleId = userRole.Id, Extension = "Chinook", Name = Permission.Write.GetPermissionName()});

                DatabaseFixture.Storage.Save();

                // 5. Build the dictionary that is used by the tool and created in GrantPermissionsController
                Dictionary<string, string> roleNameByRoleId = new Dictionary<string, string>();
                roleNameByRoleId.Add(adminRole.Id, adminRole.Name);
                roleNameByRoleId.Add(userRole.Id, userRole.Name);
                roleNameByRoleId.Add(anonymousRole.Id, anonymousRole.Name);
                roleNameByRoleId.Add(specialUserRole.Id, specialUserRole.Name);

                // Execute
                GrantViewModel model = ReadGrants.ReadAll(DatabaseFixture.RoleManager, DatabaseFixture.Storage,
                    roleNameByRoleId);

                // Assert
                // 1. Number of keys: extensions
                Assert.Equal(ExtensionManager.GetInstances<IExtensionMetadata>().Count(), model.PermissionsByRoleAndScope.Keys.Count);

                // 2. Number of roles for "Security" extension
                Assert.True((model.PermissionsByRoleAndScope.ContainsKey("SoftinuxBase.Security")));
                // We may have additional linked roles left by other tests...
                Assert.True(model.PermissionsByRoleAndScope["SoftinuxBase.Security"].Keys.Count >= 4);

                // 3. Admin role
                Assert.True(model.PermissionsByRoleAndScope["SoftinuxBase.Security"].ContainsKey(adminRole.Name));
                // Admin -> Admin, Write, Read, Never
                Assert.Equal(4, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][adminRole.Name].Count);
                Assert.Contains(Permission.Admin, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][adminRole.Name]);
                Assert.Contains(Permission.Write, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][adminRole.Name]);
                Assert.Contains(Permission.Read, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][adminRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][adminRole.Name]);

                // 4. Special User role
                Assert.True(model.PermissionsByRoleAndScope["SoftinuxBase.Security"].ContainsKey(specialUserRole.Name));
                // Write -> Write, Read, Never
                Assert.Equal(3, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][specialUserRole.Name].Count);
                Assert.Contains(Permission.Write, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][specialUserRole.Name]);
                Assert.Contains(Permission.Read, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][specialUserRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][specialUserRole.Name]);

                // 5. User role
                Assert.True(model.PermissionsByRoleAndScope["SoftinuxBase.Security"].ContainsKey(userRole.Name));

                // Read -> Read, Never
                Assert.Equal(2, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][userRole.Name].Count);
                Assert.Contains(Permission.Read, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][userRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][userRole.Name]);

                // 6. Anonymous role
                Assert.True(model.PermissionsByRoleAndScope["SoftinuxBase.Security"].ContainsKey(anonymousRole.Name));
                // Never -> Never
                Assert.Single(model.PermissionsByRoleAndScope["SoftinuxBase.Security"][anonymousRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndScope["SoftinuxBase.Security"][anonymousRole.Name]);

                // 7. Number of roles for Chinook extension
                // We may have additional linked roles left by other tests...
                Assert.True((model.PermissionsByRoleAndScope.ContainsKey("Chinook")));
                Assert.True(model.PermissionsByRoleAndScope["Chinook"].Keys.Count >= 2);

                // No need to check the details for this extension

                //  8. SoftinuxBase.SeedDatabase extension was found, no permissions should be found
                Assert.True((model.PermissionsByRoleAndScope.ContainsKey("SoftinuxBase.SeedDatabase")));
                Assert.Equal(0, model.PermissionsByRoleAndScope["SoftinuxBase.SeedDatabase"].Keys.Count);
            }
            finally
            {
                // Cleanup created data
                repo.DeleteAll();
                DatabaseFixture.Storage.Save();

                var specialUserRole = await DatabaseFixture.RoleManager.FindByNameAsync("Special User");
                await DatabaseFixture.RoleManager.DeleteAsync(specialUserRole);
            }

        }
    }
}