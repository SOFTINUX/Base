using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CommonTest;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using SeedDatabase;
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
                    {RoleId = adminRole.Id, Scope = "Security", PermissionId = Permission.Admin.GetPermissionName()});
                repo.Create(new RolePermission
                    {RoleId = userRole.Id, Scope = "Security", PermissionId = Permission.Read.GetPermissionName()});
                repo.Create(new RolePermission
                {
                    RoleId = anonymousRole.Id, Scope = "Security", PermissionId = Permission.Never.GetPermissionName()
                });
                repo.Create(new RolePermission
                {
                    RoleId = specialUserRole.Id, Scope = "Security", PermissionId = Permission.Write.GetPermissionName()
                });

                repo.Create(new RolePermission
                    {RoleId = adminRole.Id, Scope = "Chinook", PermissionId = Permission.Admin.GetPermissionName()});
                repo.Create(new RolePermission
                    {RoleId = userRole.Id, Scope = "Chinook", PermissionId = Permission.Write.GetPermissionName()});
                
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
                Assert.True((model.PermissionsByRoleAndScope.ContainsKey("Security")));
                Assert.Equal(4, model.PermissionsByRoleAndScope["Security"].Keys.Count);

                // 3. Admin role
                Assert.True(model.PermissionsByRoleAndScope["Security"].ContainsKey(adminRole.Name));
                // Admin -> Admin, Write, Read, Never
                Assert.Equal(4, model.PermissionsByRoleAndScope["Security"][adminRole.Name].Count);
                Assert.Contains(Permission.Admin, model.PermissionsByRoleAndScope["Security"][adminRole.Name]);
                Assert.Contains(Permission.Write, model.PermissionsByRoleAndScope["Security"][adminRole.Name]);
                Assert.Contains(Permission.Read, model.PermissionsByRoleAndScope["Security"][adminRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndScope["Security"][adminRole.Name]);

                // 4. Special User role
                Assert.True(model.PermissionsByRoleAndScope["Security"].ContainsKey(specialUserRole.Name));
                // Write -> Write, Read, Never
                Assert.Equal(3, model.PermissionsByRoleAndScope["Security"][specialUserRole.Name].Count);
                Assert.Contains(Permission.Write, model.PermissionsByRoleAndScope["Security"][specialUserRole.Name]);
                Assert.Contains(Permission.Read, model.PermissionsByRoleAndScope["Security"][specialUserRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndScope["Security"][specialUserRole.Name]);

                // 5. User role
                Assert.True(model.PermissionsByRoleAndScope["Security"].ContainsKey(userRole.Name));

                // Read -> Read, Never
                Assert.Equal(2, model.PermissionsByRoleAndScope["Security"][userRole.Name].Count);
                Assert.Contains(Permission.Read, model.PermissionsByRoleAndScope["Security"][userRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndScope["Security"][userRole.Name]);

                // 6. Anonymous role
                Assert.True(model.PermissionsByRoleAndScope["Security"].ContainsKey(anonymousRole.Name));
                // Never -> Never
                Assert.Single(model.PermissionsByRoleAndScope["Security"][anonymousRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndScope["Security"][anonymousRole.Name]);

                // 7. Number of roles for Chinook extension
                Assert.True((model.PermissionsByRoleAndScope.ContainsKey("Chinook")));
                Assert.Equal(2, model.PermissionsByRoleAndScope["Chinook"].Keys.Count);

                // No need to check the details for this extension

                //  8. SeedDatabase extension was found, no permissions should be found
                Assert.True((model.PermissionsByRoleAndScope.ContainsKey("SeedDatabase")));
                Assert.Equal(0, model.PermissionsByRoleAndScope["SeedDatabase"].Keys.Count);
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