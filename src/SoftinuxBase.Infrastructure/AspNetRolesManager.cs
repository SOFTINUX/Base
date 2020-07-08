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

        /// <inheritdoc />
        public async Task<IdentityRole<string>> FindByIdAsync(string roleId_)
        {
            return await base.FindByIdAsync(roleId_);
        }

        /// <inheritdoc />
        public async Task<IdentityRole<string>> FindByNameAsync(string roleName_)
        {
            return await base.FindByNameAsync(roleName_);
        }

        /// <inheritdoc />
        public async Task<IdentityResult> SetRoleNameAsync(IdentityRole<string> role_, string roleName_)
        {
            return await base.SetRoleNameAsync(role_, roleName_);
        }

        /// <inheritdoc />
        public async Task<bool> RoleExistsAsync(string roleName_)
        {
            return await base.RoleExistsAsync(roleName_);
        }

        /// <inheritdoc />
        public async Task<IdentityResult> CreateAsync(IdentityRole<string> role_)
        {
            return await base.CreateAsync(role_);
        }
    }
}