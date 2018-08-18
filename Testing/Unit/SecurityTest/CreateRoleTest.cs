// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using CommonTest;
using Microsoft.AspNetCore.Identity;
using Security.Data.Abstractions;
using Security.Tools;
using Security.ViewModels.Permissions;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class CreateRoleTest : CommonTestWithDatabase
    {
        public CreateRoleTest(DatabaseFixture databaseFixture_) : base(databaseFixture_)
        {

        }

        [Fact]
        public async void TestCheckAndSaveNewRole_Ok()
        {
            string roleName = "New Role 1 " + DateTime.Now.Ticks;
            var permRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();
            try
            {
                // Arrange
                SaveNewRoleViewModel model = new SaveNewRoleViewModel
                {
                    // Really unique value
                    RoleName = roleName,
                    Extensions = new System.Collections.Generic.List<string> { "Security" },
                    Permission = Security.Common.Enums.Permission.Write.ToString()
                };

                // Execute
                var result = await CreateRole.CheckAndSaveNewRole(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);
                Assert.Null(result);

                // Read back and assert that we have the expected data
                // 1. Expect to find the Role record for the new role
                var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(model.RoleName);
                Assert.NotNull(createdRole);

                // 2. Expect to have a single record in RolePermission table for the new role
                var rolePermissionRecords = permRepo.FilteredByRoleId(createdRole.Id);
                Assert.Single(rolePermissionRecords);
                
            }
            finally
            {
                // Cleanup created data
                var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);
                Assert.NotNull(createdRole);
                foreach (var rolePermission in permRepo.FilteredByRoleId(createdRole.Id))
                    permRepo.Delete(rolePermission.RoleId, rolePermission.Scope);

                await DatabaseFixture.RoleManager.DeleteAsync(createdRole);
            }
        }

        [Fact]
        public async void TestCheckAndSaveNewRole_NameAlreadyTaken()
        {
            string roleName = "New Role 1 " + DateTime.Now.Ticks;
            var permRepo = DatabaseFixture.Storage.GetRepository<IRolePermissionRepository>();

            try
            {
                // Arrange
                SaveNewRoleViewModel model = new SaveNewRoleViewModel
                {
                    // Really unique value
                    RoleName = roleName,
                    Extensions = new System.Collections.Generic.List<string> { "Security" },
                    Permission = Security.Common.Enums.Permission.Write.ToString()
                };

                // Execute
                var result = await CreateRole.CheckAndSaveNewRole(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);
                Assert.Null(result);

                // Read back and expect to find the Role record for the new role
                var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(model.RoleName);
                Assert.NotNull(createdRole);

                result = await CreateRole.CheckAndSaveNewRole(model, DatabaseFixture.RoleManager, DatabaseFixture.Storage);
                Assert.NotNull(result);
                Assert.Equal("A role with this name already exists", result);
            }
            finally
            {
                // Cleanup created data
                var createdRole = await DatabaseFixture.RoleManager.FindByNameAsync(roleName);
                Assert.NotNull(createdRole);
                foreach (var rolePermission in permRepo.FilteredByRoleId(createdRole.Id))
                    permRepo.Delete(rolePermission.RoleId, rolePermission.Scope);

                await DatabaseFixture.RoleManager.DeleteAsync(createdRole);
            }
        }
    }

}

