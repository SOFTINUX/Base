// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.EntityFramework;
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
        /// Check that a role with this name and another ID doesn't already exist.
        /// Then save new data into database.
        /// </summary>
        /// <param name="rolesManager_">Base's custom interface to <see cref="RoleManager{TRole}"/>.</param>
        /// <param name="model_">Model with role name.</param>
        /// <returns>Null if success, otherwise error message.</returns>
        internal static async Task<string> CheckAndUpdateRoleAsync(IAspNetRolesManager rolesManager_, UpdateRoleViewModel model_)
        {
            var role = await rolesManager_.FindByNameAsync(model_.RoleName);

            if (role != null && role.Id != model_.RoleId)
            {
                return "This name is already used by another role";
            }

            try
            {
                // Update the role name and save back to database
                var roleToUpdate = await rolesManager_.FindByIdAsync(model_.RoleId);
                await rolesManager_.SetRoleNameAsync(roleToUpdate, model_.RoleName);
                await rolesManager_.UpdateRoleAsync(roleToUpdate);
                return null;
            }
            catch (Exception e)
            {
                return $"{e.Message} {e.StackTrace}";
            }
        }
    }
}