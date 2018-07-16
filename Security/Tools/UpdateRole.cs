// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Security.Data.Abstractions;

namespace Security.Tools
{
    public static class UpdateRole
    {
        /// <summary>
        /// Check that a role with the same normalized name exists.
        /// </summary>
        /// <param name="roleName_"></param>
        /// <returns>true when a role with this normalized name is found</returns>
        public static bool CheckThatRoleOfThisNameExists(RoleManager<IdentityRole<string>> roleManager_, string roleName_, IStorage storage_)
        {
            // I wish we had roleManager_.FindByNormalizedNameAsync(roleName_) 
            return storage_.GetRepository<IAspNetRolesRepository>().FindByNormalizedName(roleManager_.NormalizeKey(roleName_));
        }
    }
}