// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using CommonTest;
using SoftinuxBase.Security.Common.Enums;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.Security.ViewModels.Permissions;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class CreateRoleTest : CommonTestWithDatabase
    {
        public CreateRoleTest(DatabaseFixture databaseFixture_) : base(databaseFixture_)
        {

        }

        /// <summary>
        /// Test the creation of role with 1 or 2 associated extensions.
        /// </summary>
        /// <param name="extensionNames_"></param>
        [Theory]
        [InlineData("Security,Chinook")]
        [InlineData("Security")]
        public async Task TestCheckAndSaveNewRole_Ok(string extensionNames_)
        {
            string roleName = "New Role 1 " + DateTime.Now.Ticks;
            var permRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();
            try
            {
                // Arrange
                string[] extensions = extensionNames_.Split(',');
                SaveNewRoleAndGrantsViewModel model = new SaveNewRoleAndGrantsViewModel
                {
                    // Really unique value
                    RoleName = roleName,
                    Extensions = new System.Collections.Generic.List<string>(extensions),
                    Permission = Permission.Write.ToString()
                };

                // Execute
                var result = await CreateRoleAndGrants.CheckAndSaveNewRoleAndGrants(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);

                // Assert
                Assert.Null(result);

                // Read back and assert that we have the expected data
                // 1. Expect to find the Role record for the new role
                var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(model.RoleName);
                Assert.NotNull(createdRole);

                // 2. Expect to have an expected number of records in RolePermission table for the new role
                var rolePermissionRecords = permRepo.FilteredByRoleId(createdRole.Id);
                Assert.Equal(extensions.Count(), rolePermissionRecords.Count());

            }
            finally
            {
                // Cleanup created data
                var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);
                if (createdRole != null)
                {
                    foreach (var rolePermission in permRepo.FilteredByRoleId(createdRole.Id))
                        permRepo.Delete(rolePermission.RoleId, rolePermission.Scope);

                    await DatabaseFixture.RoleManager.DeleteAsync(createdRole);
                }
            }
        }

        /// <summary>
        /// Test that we cannot create a role without any extension.
        /// </summary>
        [Fact]
        public async Task TestCheckAndSaveNewRole_Error_NoExtensionSelected()
        {
            string roleName = "New Role 1 " + DateTime.Now.Ticks;
            var permRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();
            try
            {
                // Arrange
                SaveNewRoleAndGrantsViewModel model = new SaveNewRoleAndGrantsViewModel
                {
                    // Really unique value
                    RoleName = roleName,
                    Extensions = null,
                    Permission = Permission.Write.ToString()
                };

                // Execute
                var result = await CreateRoleAndGrants.CheckAndSaveNewRoleAndGrants(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("At least one extension must be selected", result);
            }
            finally
            {
                // Cleanup created data
                var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);
                if (createdRole != null)
                {
                    foreach (var rolePermission in permRepo.FilteredByRoleId(createdRole.Id))
                        permRepo.Delete(rolePermission.RoleId, rolePermission.Scope);

                    await DatabaseFixture.RoleManager.DeleteAsync(createdRole);
                }
            }
        }

        [Fact]
        public async Task TestCheckAndSaveNewRole_Error_NameAlreadyTaken()
        {
            string roleName = "New Role 1 " + DateTime.Now.Ticks;
            var permRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();

            try
            {
                // Arrange
                SaveNewRoleAndGrantsViewModel model = new SaveNewRoleAndGrantsViewModel
                {
                    // Really unique value
                    RoleName = roleName,
                    Extensions = new System.Collections.Generic.List<string> { "Security" },
                    Permission = Permission.Write.ToString()
                };

                // Execute
                var result = await CreateRoleAndGrants.CheckAndSaveNewRoleAndGrants(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);

                // Assert
                Assert.Null(result);

                // Read back and expect to find the Role record for the new role
                var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(model.RoleName);
                Assert.NotNull(createdRole);

                result = await CreateRoleAndGrants.CheckAndSaveNewRoleAndGrants(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);
                Assert.NotNull(result);
                Assert.Equal("A role with this name already exists", result);
            }
            finally
            {
                // Cleanup created data
                var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);
                if (createdRole != null)
                {
                    foreach (var rolePermission in permRepo.FilteredByRoleId(createdRole.Id))
                        permRepo.Delete(rolePermission.RoleId, rolePermission.Scope);

                    await DatabaseFixture.RoleManager.DeleteAsync(createdRole);
                }
            }
        }
    }

}

