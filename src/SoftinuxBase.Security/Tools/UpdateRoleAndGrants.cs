// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.ViewModels.Permissions;

[assembly: InternalsVisibleTo("SoftinuxBase.SecurityTests")]
namespace SoftinuxBase.Security.Tools
{
    /*
       The main UpdateRoleAndGrants class.
       Contains all methods for update roles and grants.
   */
    /// <summary>
    /// The main UpdateRoleAndGrants class.
    ///
    /// Contains all methods to update roles and grants.
    /// </summary>
    /// <remarks>
    /// This class is internal to SoftinuxBase Security.
    /// </remarks>
    internal static class UpdateRoleAndGrants
    {
        /// <summary>
        /// Check that a role with the same name and another ID exists.
        /// </summary>
        /// <param name="roleManager_">Role manager object.</param>
        /// <param name="roleName_">Role name.</param>
        /// <param name="roleId_">Role ID.
        /// <remarks>When a role is found (by name), it should not have the parameter role ID.</remarks></param>
        /// <returns>True when a role is found.</returns>
        internal static async Task<bool> CheckThatRoleOfThisNameExistsAsync(RoleManager<IdentityRole<string>> roleManager_, string roleName_, string roleId_ = null)
        {
            var role = await roleManager_.FindByNameAsync(roleName_);
            return roleId_ == null ? (role != null) : (role != null && role.Id != roleId_);
        }

        /// <summary>
        /// First, check that a role with this name and another ID doesn't already exist.
        ///
        /// Second, save new data into database.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="roleManager_">Role manager.</param>
        /// <param name="model_">Model with role name and grant data (extensions and permission level).</param>
        /// <returns>Null if success, otherwise error message.</returns>
        internal static async Task<string> CheckAndUpdateRoleAndGrantsAsync(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_, UpdateRoleAndGrantsViewModel model_)
        {
            if (await CheckThatRoleOfThisNameExistsAsync(roleManager_, model_.RoleName, model_.RoleId))
            {
                return "A role with this name already exists";
            }

            // try
            // {
                // Update the role name
                var role = await roleManager_.FindByIdAsync(model_.RoleId);
                await roleManager_.SetRoleNameAsync(role, model_.RoleName);

                // TODO rewrite for new permissions
                throw new NotImplementedException();
            //     // Create the new links
            //     if (model_.Grants != null)
            //     {
            //         // Clear all the linked extensions, save the new links
            //         var permRepo = storage_.GetRepository<IRolePermissionRepository>();
            //
            //         foreach (var rolePermission in permRepo.FilteredByRoleId(model_.RoleId))
            //         {
            //             permRepo.Delete(rolePermission.RoleId, rolePermission.Extension);
            //         }
            //
            //         foreach (ExtensionPermissionValue grantData in model_.Grants)
            //         {
            //             // Convert the string to the enum
            //             if (Enum.TryParse<Permissions.Enums.Permission>(grantData.PermissionValue, true, out var permissionEnumValue))
            //             {
            //                 var permissionEntity = storage_.GetRepository<IPermissionRepository>()
            //                     .Find(permissionEnumValue);
            //
            //                 permRepo.Create(new RolePermission
            //                 {
            //                     RoleId = model_.RoleId,
            //                     PermissionId = permissionEntity.Id,
            //                     Extension = grantData.Extension
            //                 });
            //             }
            //         }
            //     }
            //
            //     await storage_.SaveAsync();
            //
            //     return null;
            // }
            // catch (Exception e)
            // {
            //     return $"{e.Message} {e.StackTrace}";
            // }
            }
    }
}