// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Security.Common.Enums;
using Security.Data.Abstractions;
using SeedDatabase;

namespace BaseTest.Common
{
    public static class DataSetupTools
    {
        /// <summary>
        /// Delete links:
        /// - betweeen roles and permissions
        /// - between users and permissions
        /// - between roles and users
        /// </summary>
        public static void DeleteAllLinks(IStorage storage_, UserManager<Security.Data.Entities.User> userManager_, RoleManager<IdentityRole<string>> roleManager_)
        {
            // Delete role-permission and user-permission links using repositories
            storage_.GetRepository<IRolePermissionRepository>().DeleteAll();
            storage_.GetRepository<IUserPermissionRepository>().DeleteAll();

            // Delete user-role permission using Identity managers
            var allUsers = userManager_.Users.ToList();
            var allRolesNames = roleManager_.Roles.Select(r => r.Name);
            foreach (var user in allUsers)
                userManager_.RemoveFromRolesAsync(user, allRolesNames);
        }

        /// <summary>
        /// If the tests database it empty, this will create the base roles: Administrator, User, Anonymous, else do nothing.
        /// Code similar to SeedDatabase controller.
        /// </summary>
        public static async void CreateBaseRolesIfNeeded(RoleManager<IdentityRole<string>> roleManager_)
        {
            // Get the list of the role from the enum
            Role[] roles = (Role[])Enum.GetValues(typeof(Role));

            foreach (var r in roles)
            {
                // create an identity role object out of the enum value
                IdentityRole<string> identityRole = new IdentityRole<string>
                {
                    Id = r.GetRoleName(),
                    Name = r.GetRoleName()
                };

                if (!await roleManager_.RoleExistsAsync(roleName: identityRole.Name))
                {
                    var result = await roleManager_.CreateAsync(identityRole);
                }
            }
        }

        /// <summary>
        /// If the tests database it empty, this will create the permissions: Admin, Write, Read, Never, else do nothing.
        /// Code similar to SeedDatabase controller.
        /// </summary>
        public static void CreatePermissionsIfNeeded(IStorage storage_)
        {
            Permission[] permissions = (Permission[])Enum.GetValues(typeof(Permission));

            foreach (var p in permissions)
            {
                // create a permission object out of the enum value
                Security.Data.Entities.Permission permission = new Security.Data.Entities.Permission()
                {
                    Id = p.GetPermissionName(),
                    Name = p.GetPermissionName()
                };

                storage_.GetRepository<IPermissionRepository>().Create(permission);
                storage_.Save();
            }
        }
    }
}