// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonTest;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.Security.ViewModels.Permissions;
using SoftinuxBase.SeedDatabase;
using Xunit;
using Constants = SoftinuxBase.Security.Common.Constants;
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
        /// User role : Write permission.
        ///
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReadAllAsync()
        {
            var repo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();
            var permRepo = DatabaseFixture.Storage.GetRepository<IPermissionRepository>();
            try
            {
                // Arrange
                // 1. Create base roles
                await CreateBaseRolesIfNeededAsync();

                // 2. Create "Special User" role
                await CreateRoleIfNotExistingAsync("Special User");

                // 3. Read roles to get their IDs
                var adminRole = await DatabaseFixture.RoleManager.FindByNameAsync(Role.Administrator.GetRoleName());
                var userRole = await DatabaseFixture.RoleManager.FindByNameAsync(Role.User.GetRoleName());
                var anonymousRole = await DatabaseFixture.RoleManager.FindByNameAsync(Role.Anonymous.GetRoleName());
                var specialUserRole = await DatabaseFixture.RoleManager.FindByNameAsync("Special User");

                // 4. Read permissions to get their IDs
                var adminPermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Admin.GetPermissionName())?.Id;
                var writePermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Write.GetPermissionName())?.Id;
                var readPermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Read.GetPermissionName())?.Id;
                var neverPermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Never.GetPermissionName())?.Id;
                Assert.NotNull(adminPermissionId);
                Assert.NotNull(writePermissionId);
                Assert.NotNull(readPermissionId);
                Assert.NotNull(neverPermissionId);

                // 5. Create role-extension links
                // Cleanup first
                repo.DeleteAll();
                await DatabaseFixture.Storage.SaveAsync();

                repo.Create(new RolePermission
                { RoleId = adminRole.Id, Extension = Constants.SoftinuxBaseSecurity, PermissionId = adminPermissionId });
                repo.Create(new RolePermission
                { RoleId = userRole.Id, Extension = Constants.SoftinuxBaseSecurity, PermissionId = readPermissionId });
                repo.Create(new RolePermission
                {
                    RoleId = anonymousRole.Id,
                    Extension = Constants.SoftinuxBaseSecurity,
                    PermissionId = neverPermissionId
                });
                repo.Create(new RolePermission
                {
                    RoleId = specialUserRole.Id,
                    Extension = Constants.SoftinuxBaseSecurity,
                    PermissionId = writePermissionId
                });

                repo.Create(new RolePermission
                { RoleId = adminRole.Id, Extension = "Chinook", PermissionId = adminPermissionId });
                repo.Create(new RolePermission
                { RoleId = userRole.Id, Extension = "Chinook", PermissionId = writePermissionId });

                await DatabaseFixture.Storage.SaveAsync();

                // 6. Build the dictionary that is used by the tool and created in GrantPermissionsController
                Dictionary<string, string> roleNameByRoleId = new Dictionary<string, string>();
                roleNameByRoleId.Add(adminRole.Id, adminRole.Name);
                roleNameByRoleId.Add(userRole.Id, userRole.Name);
                roleNameByRoleId.Add(anonymousRole.Id, anonymousRole.Name);
                roleNameByRoleId.Add(specialUserRole.Id, specialUserRole.Name);

                // Execute
                GrantViewModel model = ReadGrants.ReadAll(DatabaseFixture.RoleManager, DatabaseFixture.Storage, roleNameByRoleId);

                // Assert
                // 1. Number of keys: extensions
                Assert.Equal(ExtensionManager.GetInstances<IExtensionMetadata>().Count(), model.PermissionsByRoleAndExtension.Keys.Count);

                // 2. Number of roles for "Security" extension
                Assert.True(model.PermissionsByRoleAndExtension.ContainsKey(Constants.SoftinuxBaseSecurity));

                // We may have additional linked roles left by other tests...
                Assert.True(model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity].Keys.Count >= 4);

                // 3. Admin role
                Assert.True(model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity].ContainsKey(adminRole.Name));

                // Admin -> Admin, Write, Read, Never
                Assert.Equal(4, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][adminRole.Name].Count);
                Assert.Contains(Permission.Admin, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][adminRole.Name]);
                Assert.Contains(Permission.Write, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][adminRole.Name]);
                Assert.Contains(Permission.Read, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][adminRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][adminRole.Name]);

                // 4. Special User role
                Assert.True(model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity].ContainsKey(specialUserRole.Name));

                // Write -> Write, Read, Never
                Assert.Equal(3, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][specialUserRole.Name].Count);
                Assert.Contains(Permission.Write, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][specialUserRole.Name]);
                Assert.Contains(Permission.Read, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][specialUserRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][specialUserRole.Name]);

                // 5. User role
                Assert.True(model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity].ContainsKey(userRole.Name));

                // Read -> Read, Never
                Assert.Equal(2, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][userRole.Name].Count);
                Assert.Contains(Permission.Read, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][userRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][userRole.Name]);

                // 6. Anonymous role
                Assert.True(model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity].ContainsKey(anonymousRole.Name));

                // Never -> Never
                Assert.Single(model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][anonymousRole.Name]);
                Assert.Contains(Permission.Never, model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurity][anonymousRole.Name]);

                // 7. Number of roles for Chinook extension
                // When the dll doesn't exist on disk, this is our case, no permissions should be found
                Assert.False(model.PermissionsByRoleAndExtension.ContainsKey("Chinook"));

                // No need to check the details for this extension

                // 8. SoftinuxBase.SeedDatabase extension was found
                Assert.True(model.PermissionsByRoleAndExtension.ContainsKey("SoftinuxBase.SeedDatabase"));
            }
            finally
            {
                // Cleanup created data
                repo.DeleteAll();
                await DatabaseFixture.Storage.SaveAsync();

                var specialUserRole = await DatabaseFixture.RoleManager.FindByNameAsync("Special User");
                await DatabaseFixture.RoleManager.DeleteAsync(specialUserRole);
            }
        }

        /// <summary>
        /// Create a role, associate it to an extension with admin permission level.
        /// Create a user, associate it to the same extension with admin permission level.
        /// Run the check for the role and the extension. Expect false.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsRoleLastAdminPermissionLevelGrantForExtension_No_StillAnUserWithAdminPermissionKevelForThisExtensionAsync()
        {
            var rolePermRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();
            var permRepo = DatabaseFixture.Storage.GetRepository<IPermissionRepository>();
            var userPermRepo = DatabaseFixture.Storage.GetRepository<IUserPermissionRepository>();

            string adminPermissionId = null;
            string roleName = Guid.NewGuid().ToString();
            string extensionName = Guid.NewGuid().ToString();
            string userName = Guid.NewGuid().ToString();

            IdentityRole<string> role = null;
            User user = null;

            try
            {
                // Arrange
                // 1. Create a test role
                await CreateRoleIfNotExistingAsync(roleName);

                // 2. Read role to get its ID
                role = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);

                // 3. Read permissions to get their IDs
                adminPermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Admin.GetPermissionName())?.Id;

                // 4. Associate the role to an extension
                RolePermission rolePermission = new RolePermission { Extension = extensionName, PermissionId = adminPermissionId, RoleId = role.Id };
                rolePermRepo.Create(rolePermission);
                await DatabaseFixture.Storage.SaveAsync();

                // 5. Create an user
                user = await CreateUserAsync(userName);

                // 6. Associate the user to the extension
                UserPermission userPermission = new UserPermission { Extension = extensionName, PermissionId = adminPermissionId, UserId = user.Id };
                userPermRepo.Create(userPermission);
                await DatabaseFixture.Storage.SaveAsync();

                // Act
                bool isLast = await ReadGrants.IsRoleLastAdminPermissionLevelGrantForExtensionAsync(DatabaseFixture.RoleManager, DatabaseFixture.Storage, roleName, extensionName);

                // Assert
                Assert.False(isLast);
            }
            finally
            {
                // Cleanup
                // 1. Delete the UserPermission
                userPermRepo.Delete(user?.Id, adminPermissionId);

                // 2. Delete the RolePermission
                rolePermRepo.Delete(role?.Id, extensionName);

                // 3. Delete the User
                await DatabaseFixture.UserManager.DeleteAsync(user);

                // 4. Delete the Role
                await DatabaseFixture.RoleManager.DeleteAsync(role);
            }
        }

        /// <summary>
        /// Create a role, associate it to an extension with admin permission level.
        /// Create a user, associate it to the same extension with write permission level.
        /// Run the check for the role and the extension. Expect true.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsRoleLastAdminPermissionLevelGrantForExtension_True_NoUserWithAdminPermissionLevelForThisExtensionAsync()
        {
            var rolePermRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();
            var permRepo = DatabaseFixture.Storage.GetRepository<IPermissionRepository>();
            var userPermRepo = DatabaseFixture.Storage.GetRepository<IUserPermissionRepository>();

            string adminPermissionId = null;
            string roleName = Guid.NewGuid().ToString();
            string extensionName = Guid.NewGuid().ToString();
            string userName = Guid.NewGuid().ToString();

            IdentityRole<string> role = null;
            User user = null;

            try
            {
                // Arrange
                // 1. Create a test role
                await CreateRoleIfNotExistingAsync(roleName);

                // 2. Read role to get its ID
                role = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);

                // 3. Read permissions to get their IDs
                adminPermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Admin.GetPermissionName())?.Id;
                string writePermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Write.GetPermissionName())?.Id;

                // 4. Associate the role to an extension
                RolePermission rolePermission = new RolePermission { Extension = extensionName, PermissionId = adminPermissionId, RoleId = role.Id };
                rolePermRepo.Create(rolePermission);
                await DatabaseFixture.Storage.SaveAsync();

                // 5. Create an user
                user = await CreateUserAsync(userName);

                // 6. Associate the user to the extension
                UserPermission userPermission = new UserPermission { Extension = extensionName, PermissionId = writePermissionId, UserId = user.Id };
                userPermRepo.Create(userPermission);
                await DatabaseFixture.Storage.SaveAsync();

                // Act
                bool isLast = await ReadGrants.IsRoleLastAdminPermissionLevelGrantForExtensionAsync(DatabaseFixture.RoleManager, DatabaseFixture.Storage, roleName, extensionName);

                // Assert
                Assert.True(isLast);
            }
            finally
            {
                // Cleanup
                // 1. Delete the UserPermission
                userPermRepo.Delete(user?.Id, adminPermissionId);

                // 2. Delete the RolePermission
                rolePermRepo.Delete(role?.Id, extensionName);

                // 3. Delete the User
                await DatabaseFixture.UserManager.DeleteAsync(user);

                // 4. Delete the Role
                await DatabaseFixture.RoleManager.DeleteAsync(role);
            }
        }

        /// <summary>
        /// Create a role, associate it to an extension with admin permission level.
        /// Create a second role, associate it to an user, associate this second role to the same extension with admin permission level.
        /// Run the check for the first role and the extension. Expect false.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsRoleLastAdminPermissionLevelGrantForExtension_No_StillAnotherRoleWithAdminPermissionWithUsersForThisExtensionAsync()
        {
            var rolePermRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();
            var permRepo = DatabaseFixture.Storage.GetRepository<IPermissionRepository>();
            var userPermRepo = DatabaseFixture.Storage.GetRepository<IUserPermissionRepository>();

            string adminPermissionId = null;
            string roleName = Guid.NewGuid().ToString();
            string secondRoleName = Guid.NewGuid().ToString();
            string extensionName = Guid.NewGuid().ToString();
            string userName = Guid.NewGuid().ToString();

            IdentityRole<string> role = null;
            IdentityRole<string> secondRole = null;
            User user = null;

            try
            {
                // Arrange
                // 1. Create a test role
                await CreateRoleIfNotExistingAsync(roleName);

                // 2. Read role to get its ID
                role = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);

                // 3. Read permissions to get their IDs
                adminPermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Admin.GetPermissionName())?.Id;

                // 4. Associate the role to an extension
                RolePermission rolePermission = new RolePermission { Extension = extensionName, PermissionId = adminPermissionId, RoleId = role.Id };
                rolePermRepo.Create(rolePermission);
                await DatabaseFixture.Storage.SaveAsync();

                // 5. Create a second role
                await CreateRoleIfNotExistingAsync(secondRoleName);

                // 6. Read second role to get its ID
                secondRole = await DatabaseFixture.RoleManager.FindByNameAsync(secondRoleName);

                // 7. Create an user
                user = await CreateUserAsync(userName);

                // 8. Associate the second role to the user
                Assert.True((await DatabaseFixture.UserManager.AddToRoleAsync(user, secondRoleName)).Succeeded);

                // 9. Associate the second role to the extension
                rolePermission = new RolePermission {Extension = extensionName, PermissionId = adminPermissionId, RoleId = secondRole.Id};
                rolePermRepo.Create(rolePermission);
                await DatabaseFixture.Storage.SaveAsync();

                // Act
                bool isLast = await ReadGrants.IsRoleLastAdminPermissionLevelGrantForExtensionAsync(DatabaseFixture.RoleManager, DatabaseFixture.Storage, roleName, extensionName);

                // Assert
                Assert.False(isLast);
            }
            finally
            {
                // Cleanup
                // 1. Delete the UserPermission
                userPermRepo.Delete(user?.Id, adminPermissionId);

                // 2. Delete the RolePermission
                rolePermRepo.Delete(role?.Id, extensionName);
                rolePermRepo.Delete(secondRole?.Id, extensionName);

                // 2b. Delete the UserRole
                await DatabaseFixture.UserManager.RemoveFromRoleAsync(user, secondRoleName);

                // 3. Delete the User
                await DatabaseFixture.UserManager.DeleteAsync(user);

                // 4. Delete the Role
                await DatabaseFixture.RoleManager.DeleteAsync(role);
                await DatabaseFixture.RoleManager.DeleteAsync(secondRole);
            }
        }

        /// <summary>
        /// Create a role, associate it to an extension with admin permission level.
        /// Create a second role, associate it to an user, associate this second role to the same extension with write permission level.
        /// Run the check for the first role and the extension. Expect true.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsRoleLastAdminPermissionLevelGrantForExtension_Yes_NoAnotherRoleWithAdminPermissionWithUsersForThisExtensionAsync()
        {
            var rolePermRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();
            var permRepo = DatabaseFixture.Storage.GetRepository<IPermissionRepository>();
            var userPermRepo = DatabaseFixture.Storage.GetRepository<IUserPermissionRepository>();

            string adminPermissionId = null;
            string roleName = Guid.NewGuid().ToString();
            string secondRoleName = Guid.NewGuid().ToString();
            string extensionName = Guid.NewGuid().ToString();
            string userName = Guid.NewGuid().ToString();

            IdentityRole<string> role = null;
            IdentityRole<string> secondRole = null;
            User user = null;

            try
            {
                // Arrange
                // 1. Create a test role
                await CreateRoleIfNotExistingAsync(roleName);

                // 2. Read role to get its ID
                role = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);

                // 3. Read permissions to get their IDs
                adminPermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Admin.GetPermissionName())?.Id;
                string writePermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Write.GetPermissionName())?.Id;

                // 4. Associate the role to an extension
                RolePermission rolePermission = new RolePermission { Extension = extensionName, PermissionId = adminPermissionId, RoleId = role.Id };
                rolePermRepo.Create(rolePermission);
                await DatabaseFixture.Storage.SaveAsync();

                // 5. Create a second role
                await CreateRoleIfNotExistingAsync(secondRoleName);

                // 6. Read second role to get its ID
                secondRole = await DatabaseFixture.RoleManager.FindByNameAsync(secondRoleName);

                // 7. Create an user
                user = await CreateUserAsync(userName);

                // 8. Associate the second role to the user
                Assert.True((await DatabaseFixture.UserManager.AddToRoleAsync(user, secondRoleName)).Succeeded);

                // 9. Associate the second role to the extension
                rolePermission = new RolePermission { Extension = extensionName, PermissionId = writePermissionId, RoleId = secondRole.Id };
                rolePermRepo.Create(rolePermission);
                await DatabaseFixture.Storage.SaveAsync();

                // Act
                bool isLast = await ReadGrants.IsRoleLastAdminPermissionLevelGrantForExtensionAsync(DatabaseFixture.RoleManager, DatabaseFixture.Storage, roleName, extensionName);

                // Assert
                Assert.True(isLast);
            }
            finally
            {
                // Cleanup
                // 1. Delete the UserPermission
                userPermRepo.Delete(user?.Id, adminPermissionId);

                // 2. Delete the RolePermission
                rolePermRepo.Delete(role?.Id, extensionName);
                rolePermRepo.Delete(secondRole?.Id, extensionName);

                // 2b. Delete the UserRole
                await DatabaseFixture.UserManager.RemoveFromRoleAsync(user, secondRoleName);

                // 3. Delete the User
                await DatabaseFixture.UserManager.DeleteAsync(user);

                // 4. Delete the Role
                await DatabaseFixture.RoleManager.DeleteAsync(role);
                await DatabaseFixture.RoleManager.DeleteAsync(secondRole);
            }
        }

        /// <summary>
        /// Create a role, associate it to an extension with admin permission level.
        /// Create a second role, associate this second role to the same extension with admin permission level. The role is not associated to users.
        /// Run the check for the first role and the extension. Expect true.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsRoleLastAdminPermissionLevelGrantForExtension_Yes_StillAnotherRoleButWithoutUsersForThisExtensionAsync()
        {
            var rolePermRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();
            var permRepo = DatabaseFixture.Storage.GetRepository<IPermissionRepository>();

            string roleName = Guid.NewGuid().ToString();
            string secondRoleName = Guid.NewGuid().ToString();
            string extensionName = Guid.NewGuid().ToString();

            IdentityRole<string> role = null;
            IdentityRole<string> secondRole = null;

            try
            {
                // Arrange
                // 1. Create a test role
                await CreateRoleIfNotExistingAsync(roleName);

                // 2. Read role to get its ID
                role = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);

                // 3. Read permissions to get their IDs
                string adminPermissionId = permRepo.All().FirstOrDefault(p_ => p_.Name == Permission.Admin.GetPermissionName())?.Id;

                // 4. Associate the role to an extension
                RolePermission rolePermission = new RolePermission { Extension = extensionName, PermissionId = adminPermissionId, RoleId = role.Id };
                rolePermRepo.Create(rolePermission);
                await DatabaseFixture.Storage.SaveAsync();

                // 5. Create a second role
                await CreateRoleIfNotExistingAsync(secondRoleName);

                // 6. Read second role to get its ID
                secondRole = await DatabaseFixture.RoleManager.FindByNameAsync(secondRoleName);

                // 7. Associate the second role to the extension
                rolePermission = new RolePermission { Extension = extensionName, PermissionId = adminPermissionId, RoleId = secondRole.Id };
                rolePermRepo.Create(rolePermission);
                await DatabaseFixture.Storage.SaveAsync();

                // Act
                bool isLast = await ReadGrants.IsRoleLastAdminPermissionLevelGrantForExtensionAsync(DatabaseFixture.RoleManager, DatabaseFixture.Storage, roleName, extensionName);

                // Assert
                Assert.True(isLast);
            }
            finally
            {
                // Cleanup
                
                // 1. Delete the RolePermission
                rolePermRepo.Delete(role?.Id, extensionName);
                rolePermRepo.Delete(secondRole?.Id, extensionName);

                // 2. Delete the Role
                await DatabaseFixture.RoleManager.DeleteAsync(role);
                await DatabaseFixture.RoleManager.DeleteAsync(secondRole);
            }
        }

    }
}