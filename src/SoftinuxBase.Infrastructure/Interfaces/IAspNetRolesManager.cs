// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SoftinuxBase.Infrastructure.Interfaces
{
    /// <summary>
    /// This interface allows to wrap <see cref="RoleManager{TRole}">Asp.net Identity Role Manager</see> with an interface
    /// so that it's easier to mock in unit tests with a specific interface.
    /// </summary>
    public interface IAspNetRolesManager
    {
        /// <summary>
        /// Gets all the roles, as an IQueryable collection.
        /// </summary>
        IQueryable<IdentityRole<string>> Roles { get; }

        /// <summary>
        /// Find a role by id.
        /// </summary>
        /// <param name="roleId_">Role (GU)ID.</param>
        /// <returns>The found role or null.</returns>
        Task<IdentityRole<string>> FindByIdAsync(string roleId_);

        /// <summary>
        /// Find a role by name, ignoring case.
        /// </summary>
        /// <param name="roleName_">Role name.</param>
        /// <returns>The found role or null.</returns>
        Task<IdentityRole<string>> FindByNameAsync(string roleName_);

        /// <summary>
        /// Update a role name.
        /// </summary>
        /// <param name="role_">Role to update.</param>
        /// <param name="roleName_">New role name.</param>
        /// <returns>Operation result.</returns>
        Task<IdentityResult> SetRoleNameAsync(IdentityRole<string> role_, string roleName_);

        /// <summary>
        /// Save to database the modified data of this role.
        /// </summary>
        /// <param name="role_">Role.</param>
        /// <returns>Operation result.</returns>
        Task<IdentityResult> UpdateRoleAsync(IdentityRole<string> role_);

        /// <summary>
        /// Check existence of a role, by name, ignoring case.
        /// </summary>
        /// <param name="roleName_">Role name.</param>
        /// <returns>true when a role was found.</returns>
        Task<bool> RoleExistsAsync(string roleName_);

        /// <summary>
        /// Create a role.
        /// </summary>
        /// <param name="role_">Role.</param>
        /// <returns>Operation result.</returns>
        Task<IdentityResult> CreateAsync(IdentityRole<string> role_);
    }
}