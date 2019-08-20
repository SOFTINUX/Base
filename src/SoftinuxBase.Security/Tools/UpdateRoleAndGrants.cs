// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.ViewModels.Permissions;

[assembly: InternalsVisibleTo("SecurityTest")]
namespace SoftinuxBase.Security.Tools
{
    internal static class UpdateRoleAndGrants
    {
        /// <summary>
        /// Check that a role with the same normalized name exists.
        /// </summary>
        /// <param name="roleManager_">asp identity role manager object.</param>
        /// <param name="roleName_">Role name.</param>
        /// <param name="roleId_">Role ID. When not null, the found role should not have this id.</param>
        /// <returns>true when a role with this normalized name is found.</returns>
        internal static async Task<bool> CheckThatRoleOfThisNameExists(RoleManager<IdentityRole<string>> roleManager_, string roleName_, string roleId_ = null)
        {
            var role = await roleManager_.FindByNameAsync(roleName_);
            return roleId_ == null ? (role != null) : (role != null && role.Id != roleId_);
        }

        /// <summary>
        /// First, check that a role with this name and another ID doesn't already exist.
        /// Second, save new data into database.
        /// </summary>
        /// <param name="storage_">The data storage instance.</param>
        /// <param name="roleManager_">ASP.NET Core identity role manager.</param>
        /// <param name="model_">Model with role name and grant data (extensions and permission level).</param>
        /// <returns>Current asynchronous Task with not null string result when something failed, else null when save went ok.</returns>
        internal static async Task<string> CheckAndUpdateRoleAndGrants(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_, UpdateRoleAndGrantsViewModel model_)
        {
            if (await CheckThatRoleOfThisNameExists(roleManager_, model_.RoleName, model_.RoleId))
            {
                return "A role with this name already exists";
            }

            try
            {
                // Update the role name
                var role = await roleManager_.FindByIdAsync(model_.RoleId);
                await roleManager_.SetRoleNameAsync(role, model_.RoleName);

                // Create the new links
                if (model_.Grants != null)
                {
                    // Clear all the linked extensions, save the new links
                    var permRepo = storage_.GetRepository<IRolePermissionRepository>();

                    foreach (var rolePermission in permRepo.FilteredByRoleId(model_.RoleId))
                    {
                        permRepo.Delete(rolePermission.RoleId, rolePermission.Extension);
                    }

                    foreach (ExtensionPermissionValue grantData in model_.Grants)
                    {
                        // Convert the string to the enum
                        if (Enum.TryParse<Common.Enums.Permission>(grantData.PermissionValue, true, out var permissionEnumValue))
                        {
                            var permissionEntity = storage_.GetRepository<IPermissionRepository>()
                                .Find(permissionEnumValue);

                            permRepo.Create(new RolePermission
                            {
                                RoleId = model_.RoleId,
                                PermissionId = permissionEntity.Id,
                                Extension = grantData.Extension
                            });
                        }
                    }
                }

                await storage_.SaveAsync();

                return null;
            }
            catch (Exception e)
            {
                return $"{e.Message} {e.StackTrace}";
            }
        }
    }
}