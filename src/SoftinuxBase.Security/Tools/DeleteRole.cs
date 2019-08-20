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
    internal static class DeleteRole
    {
        /// <summary>
        /// Delete the record indicating that a role is linked to an extension.
        /// </summary>
        /// <param name="storage_">the data storage instance.</param>
        /// <param name="roleManager_">role manager instance.</param>
        /// <param name="extensionName_">string represent extension name.</param>
        /// <param name="roleName_">string represent role name.</param>
        /// <returns>false when record to delete wasn't found.</returns>
        internal static async Task<bool> DeleteRoleExtensionLink(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_, string extensionName_, string roleName_)
        {
            string roleId = (await roleManager_.FindByNameAsync(roleName_)).Id;
            IRolePermissionRepository repo = storage_.GetRepository<IRolePermissionRepository>();
            if (repo.FindBy(roleId, extensionName_) == null)
            {
                return false;
            }

            repo.Delete(roleId, extensionName_);
            storage_.Save();
            return true;

        }

        /// <summary>
        /// Delete all the records indicating that a role is linked to extensions.
        /// </summary>
        /// <param name="storage_">the data storage instance.</param>
        /// <param name="roleManager_">role manager instance.</param>
        /// <param name="roleName_">string represent role name.</param>
        /// <returns>false when records to delete weren't found.</returns>
        internal static async Task<bool> DeleteRoleExtensionLinks(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_, string roleName_)
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

            storage_.Save();
            return true;
        }

        internal static async Task<string> DeleteRoleAndAllLinks(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_, string roleName_)
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
            await DeleteRoleExtensionLinks(storage_, roleManager_, roleName_);

            // delete the role-users links
            // TODO use UserManager.RemoveFromRoleAsync - make a new method that may be reused
            // delete the role itself
            await roleManager_.DeleteAsync(await roleManager_.FindByNameAsync(roleName_));
            return null;
        }
    }
}