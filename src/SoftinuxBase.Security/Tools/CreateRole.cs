// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.ViewModels.Permissions;

namespace SoftinuxBase.Security.Tools
{
    /// <summary>
    /// The main CreateRoleAndGrants class.
    ///
    /// Contains all methods for reading granting permissions.
    /// </summary>
    public static class CreateRole
    {
        /// <summary>
        /// Check that a role with this name doesn't already exist then save new role into database.
        /// </summary>
        /// <param name="rolesManager_">Base's custom interface to <see cref="RoleManager{TRole}"/>.</param>
        /// <param name="model_">A <see cref="SaveNewRoleViewModel" /> object.</param>
        /// <returns>Null if success, otherwise error message.</returns>
        public static async Task<string> CheckAndSaveNewRoleAsync(IAspNetRolesManager rolesManager_, SaveNewRoleViewModel model_)
        {
            if (await rolesManager_.FindByNameAsync(model_.RoleName) != null)
            {
                return "A role with this name already exists";
            }

            try
            {
                // Save the Role
                IdentityRole<string> identityRole = new IdentityRole<string>
                {
                    // Auto-incremented ID
                    Name = model_.RoleName
                };
                await rolesManager_.CreateAsync(identityRole);

                return null;
            }
            catch (Exception e)
            {
                return $"{e.Message} {e.StackTrace}";
            }
        }
    }
}