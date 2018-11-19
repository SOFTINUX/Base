// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using CommonTest;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.Security.ViewModels.Permissions;
using Xunit;
using Permission = SoftinuxBase.Security.Common.Enums.Permission;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class UpdateRoleTest : CommonTestWithDatabase
    {
        public UpdateRoleTest(DatabaseFixture databaseFixture_) : base(databaseFixture_)
        {

        }

        /// <summary>
        /// The role name is updated, a permission to an extension is added.
        /// The changes are prevented because the new role name isn't available.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestCheckAndUpdateRole_Error_NameAlreadyTaken()
        {
            string firstRoleName = "New Role 1 " + DateTime.Now.Ticks;
            string secondRoleName = "New Role 2 " + DateTime.Now.Ticks;
            var permRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();

            try
            {
                // Arrange
                IdentityRole<string> firstRole = new IdentityRole<string>
                {
                    // Auto-incremented ID
                    Name = firstRoleName
                };
                await DatabaseFixture.RoleManager.CreateAsync(firstRole);

                IdentityRole<string> secondRole = new IdentityRole<string>
                {
                    // Auto-incremented ID
                    Name = secondRoleName
                };
                await DatabaseFixture.RoleManager.CreateAsync(secondRole);

                // Get back the second role ID
                string secondRoleId = (await DatabaseFixture.RoleManager.FindByNameAsync(secondRoleName)).Id;

                UpdateRoleAndGrantsViewModel model = new UpdateRoleAndGrantsViewModel
                {
                    RoleId = secondRoleId,
                    // Use the first role name
                    RoleName = firstRoleName,
                    Extensions = new System.Collections.Generic.List<string> { "Security" },
                    Permission = Permission.Write.ToString()
                };

                // Execute
                var result = await UpdateRoleAndGrants.CheckAndUpdateRoleAndGrants(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("A role with this name already exists", result);

                // We should not find linked extension
                var record = permRepo.FilteredByRoleId(secondRoleId).FirstOrDefault();
                Assert.Null(record);

            }
            finally
            {
                // Cleanup created data
                string[] roleNames = { firstRoleName, secondRoleName };
                foreach (string roleName in roleNames)
                {
                    var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);
                    if (createdRole == null)
                        continue;
                    foreach (var rolePermission in permRepo.FilteredByRoleId(createdRole.Id))
                        permRepo.Delete(rolePermission.RoleId, rolePermission.Extension);

                    await DatabaseFixture.RoleManager.DeleteAsync(createdRole);
                }
            }
        }

        /// <summary>
        /// The role name is updated, a permission to an extension is added.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestCheckAndUpdateRole_NameAvailable_AddExtension()
        {
            string firstRoleName = "New Role 1 " + DateTime.Now.Ticks;
            string secondRoleName = "New Role 2 " + DateTime.Now.Ticks;
            string thirdRoleName = "New Role 3 " + DateTime.Now.Ticks;
            var permRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();

            try
            {
                // Arrange
                IdentityRole<string> firstRole = new IdentityRole<string>
                {
                    // Auto-incremented ID
                    Name = firstRoleName
                };
                await DatabaseFixture.RoleManager.CreateAsync(firstRole);

                IdentityRole<string> secondRole = new IdentityRole<string>
                {
                    // Auto-incremented ID
                    Name = secondRoleName
                };
                await DatabaseFixture.RoleManager.CreateAsync(secondRole);

                // Get back the second role ID
                string secondRoleId = (await DatabaseFixture.RoleManager.FindByNameAsync(secondRoleName)).Id;

                UpdateRoleAndGrantsViewModel model = new UpdateRoleAndGrantsViewModel
                {
                    RoleId = secondRoleId,
                    // Use another role name
                    RoleName = thirdRoleName,
                    Extensions = new System.Collections.Generic.List<string> { "Security" },
                    Permission = Permission.Write.ToString()
                };

                // Execute
                var result = await UpdateRoleAndGrants.CheckAndUpdateRoleAndGrants(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);

                // Assert
                Assert.Null(result);

                // We should find one linked extension, "Security", with Write level
                var record = permRepo.FilteredByRoleId(secondRoleId).FirstOrDefault();
                Assert.NotNull(record);
                Assert.Equal("Security", record.Extension);
                Assert.Equal(Permission.Write.ToString(), record.PermissionId);
            }
            finally
            {
                // Cleanup created data
                string[] roleNames = { firstRoleName, secondRoleName, thirdRoleName };
                foreach (string roleName in roleNames)
                {
                    var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);
                    if (createdRole == null)
                        continue;
                    foreach (var rolePermission in permRepo.FilteredByRoleId(createdRole.Id))
                        permRepo.Delete(rolePermission.RoleId, rolePermission.Extension);

                    await DatabaseFixture.RoleManager.DeleteAsync(createdRole);
                }
            }
        }

        /// <summary>
        /// A permission to an extension is deleted, another is added, another is modified.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestCheckAndUpdateRole_ChangeAddDeleteExtension()
        {
            string roleName = "New Role 1 " + DateTime.Now.Ticks;
            var permRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();

            try
            {
                // Arrange
                IdentityRole<string> firstRole = new IdentityRole<string>
                {
                    // Auto-incremented ID
                    Name = roleName
                };
                await DatabaseFixture.RoleManager.CreateAsync(firstRole);

                // Get back the second role ID
                string roleId = (await DatabaseFixture.RoleManager.FindByNameAsync(roleName)).Id;

                // Add a link to an extension
                permRepo.Create(new RolePermission { Name = Permission.Read.ToString(), RoleId = roleId, Extension = "Security" });
                permRepo.Create(new RolePermission { Name = Permission.Read.ToString(), RoleId = roleId, Extension = "Another" });
                DatabaseFixture.Storage.Save();

                UpdateRoleAndGrantsViewModel model = new UpdateRoleAndGrantsViewModel
                {
                    RoleId = roleId,
                    RoleName = roleName,
                    Extensions = new System.Collections.Generic.List<string> { "Security", "ThirdExtension" },
                    Permission = Permission.Write.ToString()
                };

                // Execute
                var result = await UpdateRoleAndGrants.CheckAndUpdateRoleAndGrants(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);

                // Assert
                Assert.Null(result);

                // We should find two linked extensions
                var records = permRepo.FilteredByRoleId(roleId);
                var rolePermissions = records as RolePermission[] ?? records.ToArray();
                Assert.Equal(2, rolePermissions.Length);
                var record = rolePermissions.FirstOrDefault(r_ => r_.Extension == "Security");
                Assert.NotNull(record);
                Assert.Equal(Permission.Write.ToString(), record.PermissionId);

                record = rolePermissions.FirstOrDefault(r_ => r_.Extension == "ThirdExtension");
                Assert.NotNull(record);
                Assert.Equal(Permission.Write.ToString(), record.PermissionId);
            }
            finally
            {
                // Cleanup created data

                var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);
                if (createdRole != null)
                {
                    foreach (var rolePermission in permRepo.FilteredByRoleId(createdRole.Id))
                        permRepo.Delete(rolePermission.RoleId, rolePermission.Extension);

                    await DatabaseFixture.RoleManager.DeleteAsync(createdRole);
                }
            }
        }

        /// <summary>
        /// A null extensions list is passed, there will be no change.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestCheckAndUpdateRole_NoExtensionInList_NoChange()
        {
            string firstRoleName = "New Role 1 " + DateTime.Now.Ticks;
            var permRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();

            try
            {
                // Arrange
                IdentityRole<string> firstRole = new IdentityRole<string>
                {
                    // Auto-incremented ID
                    Name = firstRoleName
                };
                await DatabaseFixture.RoleManager.CreateAsync(firstRole);

                // Create a link to an extension
                permRepo.Create(new RolePermission { RoleId = firstRole.Id, Name = Permission.Write.ToString(), Extension = "Security"});
                DatabaseFixture.Storage.Save();

                UpdateRoleAndGrantsViewModel model = new UpdateRoleAndGrantsViewModel
                {
                    RoleId = firstRole.Id,
                    // Use same role name
                    RoleName = firstRoleName,
                    Extensions = null,
                    Permission = Permission.Write.ToString()
                };

                // Execute
                var result = await UpdateRoleAndGrants.CheckAndUpdateRoleAndGrants(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);

                // Assert
                Assert.Null(result);

                // We should find one linked extension, "Security", with Write level
                var record = permRepo.FilteredByRoleId(firstRole.Id).FirstOrDefault();
                Assert.NotNull(record);
                Assert.Equal("Security", record.Extension);
                Assert.Equal(Permission.Write.ToString(), record.PermissionId);
            }
            finally
            {
                // Cleanup created data
                string[] roleNames = { firstRoleName };
                foreach (string roleName in roleNames)
                {
                    var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);
                    if (createdRole == null)
                        continue;
                    foreach (var rolePermission in permRepo.FilteredByRoleId(createdRole.Id))
                        permRepo.Delete(rolePermission.RoleId, rolePermission.Extension);

                    await DatabaseFixture.RoleManager.DeleteAsync(createdRole);
                }
            }
        }

    }
}

