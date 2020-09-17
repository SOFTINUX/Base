// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Security.ViewModels.Permissions;

[assembly: InternalsVisibleTo("SoftinuxBase.SecurityTests")]

namespace SoftinuxBase.Security.Tools
{
    /// <summary>
    /// The UpdateRole class contains all methods to update roles.
    /// </summary>
    /// <remarks>
    /// This class is internal to SoftinuxBase Security.
    /// </remarks>
    internal static class UpdateRole
    {
        /// <summary>
        /// Check that a role with the same name and another ID doesn't exists.
        /// </summary>
        /// <param name="rolesManager_">Base's custom interface to <see cref="RoleManager{TRole}"/>.</param>
        /// <param name="roleName_">Role name.</param>
        /// <param name="roleId_">Role ID.</param>
        /// <returns>True when a role is not found or is found with parameter ID.</returns>
        internal static async Task<bool> IsRoleNameAvailableAsync(IAspNetRolesManager rolesManager_, string roleName_, string roleId_)
        {
            var role = await rolesManager_.FindByNameAsync(roleName_);
            return role == null || role.Id == roleId_;
        }

        // TOTEST

        /// <summary>
        /// Check that a role with this name and another ID doesn't already exist.
        /// Then save new data into database.
        /// </summary>
        /// <param name="rolesManager_">Base's custom interface to <see cref="RoleManager{TRole}"/>.</param>
        /// <param name="model_">Model with role name.</param>
        /// <returns>Null if success, otherwise error message.</returns>
        internal static async Task<string> CheckAndUpdateRoleAsync(IAspNetRolesManager rolesManager_, UpdateRoleViewModel model_)
        {
            if (!(await IsRoleNameAvailableAsync(rolesManager_, model_.RoleName, model_.RoleId)))
            {
                return "This name is already used by another role";
            }

            try
            {
                // Update the role name
                var role = await rolesManager_.FindByIdAsync(model_.RoleId);
                await rolesManager_.SetRoleNameAsync(role, model_.RoleName);

                return null;
            }
            catch (Exception e)
            {
                return $"{e.Message} {e.StackTrace}";
            }
        }
    }
}