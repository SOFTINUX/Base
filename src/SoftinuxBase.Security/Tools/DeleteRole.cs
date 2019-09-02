// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security.Tools
{
    /*
        The main Deletion class.
        Contains all methods for deletions.
    */
    /// <summary>
    /// The main Deletion class.
    ///
    /// Contains all methods for deletions.
    /// </summary>
    internal static class DeleteRole
    {
        /// <summary>
        /// Delete a link between a role and an extension
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="roleManager_">Roles manager instance.</param>
        /// <param name="extensionName_">Extension name.</param>
        /// <param name="roleName_">Role name.</param>
        /// <returns>Return null on succes, otherwise return error message.</returns>
        internal static async Task<bool> DeleteRoleExtensionLinkAsync(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_, string extensionName_, string roleName_)
        {
            string roleId = (await roleManager_.FindByNameAsync(roleName_)).Id;
            IRolePermissionRepository repo = storage_.GetRepository<IRolePermissionRepository>();
            if (repo.FindBy(roleId, extensionName_) == null)
            {
                return false;
            }

            repo.Delete(roleId, extensionName_);
            await storage_.SaveAsync();
            return true;
        }

        /// <summary>
        /// Delete all links between a role and extensions.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="roleManager_">Roles manager instance.</param>
        /// <param name="roleName_">Role name.</param>
        /// <returns>Return false if not found, otherwise return true.</returns>
        internal static async Task<bool> DeleteRoleExtensionLinksAsync(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_, string roleName_)
        {
            string roleId = (await roleManager_.FindByNameAsync(roleName_)).Id;
            IRolePermissionRepository repo = storage_.GetRepository<IRolePermissionRepository>();
            IEnumerable<RolePermission> records = repo.FilteredByRoleId(roleId).ToList();
            if (!records.Any())
            {
                return false;
            }

            foreach (var record in records)
            {
                repo.Delete(record.RoleId, record.Extension);
            }

            await storage_.SaveAsync();
            return true;
        }

        /// <summary>
        /// Delete role and all links to extensions.
        /// </summary>
        /// <remarks>⚠️This method is under development.</remarks>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="roleManager_">Roles manager instance.</param>
        /// <param name="roleNameList_">Role name.</param>
        /// <returns>null</returns>
        internal static async Task<string> DeleteRoleAndAllLinksAsync(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_, List<string> roleNameList_)
        {
            /*bool canDeleteRole = false;
            string cannotDeleteMessage = null;
            string roleId = (await roleManager_.FindByNameAsync(roleName_)).Id;
            bool hasAnyUserDirectAdminPermission = ReadGrants.HasAnyUserDirectAdminPermission();
            if (hasAnyUserDirectAdminPermission)
            {
                // We can delete this role, a user has Admin permission directly granted
                canDeleteRole = true;
            }
            else
            {
                bool hasOtherRoleAdminPermission = ReadGrants.HasOtherRoleAdminPermission(roleId);
                if (!hasOtherRoleAdminPermission)
                {
                    // We can not delete this role, no other role has Admin permission directly granted
                    canDeleteRole = false;
                    cannotDeleteMessage = "The role cannot be deleted, it's the last role with Admin permission";
                }
            }

            if (!canDeleteRole)
            {
                return cannotDeleteMessage;
            }*/

            // delete the role-extensions links
            foreach (var role_ in roleNameList_)
            {
                //await DeleteRoleExtensionLinksAsync(storage_, roleManager_, role_);
                // delete the role-users links
                // TODO use UserManager.RemoveFromRoleAsync - make a new method that may be reused
                // delete the role itself
                //await roleManager_.DeleteAsync(await roleManager_.FindByNameAsync(role_));
            }

            return null;
        }
    }
}