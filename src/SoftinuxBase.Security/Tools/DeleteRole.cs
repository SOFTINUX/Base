// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.Abstractions;

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
        /// Delete all permissions granted to a role.
        /// </summary>
        /// <param name="rolesManager_">Base's custom interface to <see cref="RoleManager{TRole}"/>.</param>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="roleName_">Role name.</param>
        /// <returns>False if not data to delete found, otherwise true.</returns>
        internal static async Task<bool?> DeleteRolePermissionsAsync(IAspNetRolesManager rolesManager_, IStorage storage_, string roleName_)
        {
            // TOTEST
            string roleId = (await rolesManager_.FindByNameAsync(roleName_))?.Id;
            if (string.IsNullOrEmpty(roleId))
            {
                return null;
            }

            IRoleToPermissionsRepository repo = storage_.GetRepository<IRoleToPermissionsRepository>();
            var record = repo.FindBy(roleName_);

            if (record == null)
            {
                return null;
            }

            repo.Delete(record);

            await storage_.SaveAsync();

            return true;
        }

        /// <summary>
        /// Delete role and all links to extensions.
        /// </summary>
        /// <remarks>This method is under development.</remarks>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="roleName_">Role name.</param>
        /// <returns>null.</returns>
        internal static async Task<string> DeleteRoleAndAllLinksAsync(IStorage storage_, string roleName_)
        {
            // TODO rewrite for new permissions

            // // delete the role-extensions links
            // await DeleteRoleExtensionsLinksAsync(storage_, roleManager_, roleName_);
            //
            // // TODO use UserManager.RemoveFromRoleAsync - make a new method that may be reused
            //
            // // delete the role itself
            // await roleManager_.DeleteAsync(await roleManager_.FindByNameAsync(roleName_));
            //
            // return null;
            await storage_.SaveAsync();
            throw new NotImplementedException();
        }
    }
}