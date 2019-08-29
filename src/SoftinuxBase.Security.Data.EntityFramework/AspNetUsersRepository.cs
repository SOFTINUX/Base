// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    /// <summary>
    /// A class for performing queries related to <see cref="User"/>.
    /// </summary>
    public class AspNetUsersRepository : RepositoryBase<User>, IAspNetUsersRepository
    {
        /// <summary>
        /// Check by name or e-mail user existence.
        /// </summary>
        /// <param name="normalizedValue_">String value from <see cref="UserManager{TUser}.NormalizeKey"/>.</param>
        /// <returns>A bool indicating that an user was found.</returns>
        public bool FindByNormalizedUserNameOrEmail(string normalizedValue_)
        {
            return dbSet.FirstOrDefault(e_ => e_.NormalizedUserName == normalizedValue_ || e_.NormalizedEmail == normalizedValue_) != null;
        }

        /// <summary>
        /// Find all the users linked to roles.
        /// </summary>
        /// <param name="roleNames_"><see cref="IEnumerable{String}"/> role names.</param>
        /// <returns><see cref="IEnumerable{User}"/> users.</returns>
        public IEnumerable<User> FindActiveUsersHavingRoles(IEnumerable<string> roleNames_)
        {
            IEnumerable<string> normalizedRoleNames = roleNames_.Select(n_ => n_.ToUpperInvariant());

            // TODO add another where clause "where u. ... == ..." to keep only active users
            IEnumerable<User> users =
                from u in storageContext.Set<User>()
                join ur in storageContext.Set<IdentityUserRole<string>>() on u.Id equals ur.UserId
                join r in storageContext.Set<IdentityRole<string>>() on ur.RoleId equals r.Id
                where normalizedRoleNames.Contains(r.NormalizedName)
                select u;

            return users;
        }
    }
}