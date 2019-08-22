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
    public class AspNetUsersRepository : RepositoryBase<User>, IAspNetUsersRepository
    {
        /// <summary>
        /// Return true if found, else false.
        /// </summary>
        /// <param name="value_">normalized string to find.</param>
        /// <returns>bool.</returns>
        public bool FindByNormalizedUserNameOrEmail(string value_)
        {
            return dbSet.FirstOrDefault(e_ => e_.NormalizedUserName == value_ || e_.NormalizedEmail == value_) != null;
        }

        /// <summary>
        /// Find all the users that have roles defined by their names.
        /// </summary>
        /// <param name="roleNames_">Name of roles.</param>
        /// <returns>Linked users.</returns>
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