// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Security.Tools
{
    public static class UpdateRole
    {
        /// <summary>
        /// Check that a role with the same normalized name exists.
        /// </summary>
        /// <param name="roleName_"></param>
        /// <returns>true when a role with this normalized name is found</returns>
        public static async Task<bool> CheckThatRoleOfThisNameExists(RoleManager<IdentityRole<string>> roleManager_, string roleName_)
        {
            return await roleManager_.FindByNameAsync(roleName_) != null;
        }
    }
}