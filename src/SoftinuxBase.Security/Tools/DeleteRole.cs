// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.ViewModels.Permissions;

namespace SoftinuxBase.Security.Tools
{
    public static class DeleteRole
    {
        /// <summary>
        /// Delete the record indicating that a role is linked to an extension.
        /// </summary>
        /// <param name="roleName_"></param>
        /// <param name="scope_"></param>
        /// <param name="roleManager_"></param>
        /// <param name="storage_"></param>
        /// <returns>boolean</returns>
        public static async Task<bool> DeleteRoleExtensionLink(string roleName_, string scope_, RoleManager<IdentityRole<string>> roleManager_, IStorage storage_)
        {
            string roleId = (await roleManager_.FindByNameAsync(roleName_)).Id;
            IRolePermissionRepository repo = storage_.GetRepository<IRolePermissionRepository>();
            if (repo.FindBy(roleId, scope_) != null)
            {
                repo.Delete(roleId, scope_);
                storage_.Save();
                return true;
            }
            return false;
        }

        public static async Task<string> DeleteRoleAndGrants(string roleName_, RoleManager<IdentityRole<string>> roleManager_)
        {
            string roleId = (await roleManager_.FindByNameAsync(roleName_)).Id;
            return "Work in progress. RoleId value: " + roleId;
        }
    }
}