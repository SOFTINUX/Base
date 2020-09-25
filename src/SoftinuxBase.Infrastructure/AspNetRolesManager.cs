// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Infrastructure
{
    /// <summary>
    /// This class allows to access <see cref="RoleManager{TRole}">Asp.net Identity Role Manager</see> functions.
    /// </summary>
    public class AspNetRolesManager : RoleManager<IdentityRole<string>>, IAspNetRolesManager
    {
        public AspNetRolesManager(IRoleStore<IdentityRole<string>> store_, IEnumerable<IRoleValidator<IdentityRole<string>>> roleValidators_, ILookupNormalizer keyNormalizer_, IdentityErrorDescriber errors_, ILogger<RoleManager<IdentityRole<string>>> logger_)
            : base(store_, roleValidators_, keyNormalizer_, errors_, logger_)
        {
        }

        /// <inheritdoc cref="IAspNetRolesManager.FindByIdAsync" />
        public async override Task<IdentityRole<string>> FindByIdAsync(string roleId_)
        {
            return await base.FindByIdAsync(roleId_);
        }

        /// <inheritdoc cref="IAspNetRolesManager.FindByNameAsync" />
        public async override Task<IdentityRole<string>> FindByNameAsync(string roleName_)
        {
            return await base.FindByNameAsync(roleName_);
        }

        /// <inheritdoc cref="IAspNetRolesManager.SetRoleNameAsync" />
        public async override Task<IdentityResult> SetRoleNameAsync(IdentityRole<string> role_, string roleName_)
        {
            return await base.SetRoleNameAsync(role_, roleName_);
        }

        /// <inheritdoc cref="IAspNetRolesManager.RoleExistsAsync" />
        public async override Task<bool> RoleExistsAsync(string roleName_)
        {
            return await base.RoleExistsAsync(roleName_);
        }

        /// <inheritdoc cref="IAspNetRolesManager.CreateAsync" />
        public async override Task<IdentityResult> CreateAsync(IdentityRole<string> role_)
        {
            return await base.CreateAsync(role_);
        }

        /// <inheritdoc cref="IAspNetRolesManager.UpdateRoleAsync" />
        public async new Task<IdentityResult> UpdateRoleAsync(IdentityRole<string> role_)
        {
            return await base.UpdateRoleAsync(role_);
        }
    }
}