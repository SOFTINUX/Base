// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
namespace SoftinuxBase.Security.Data.EntityFramework
{
    /// <summary>
    /// A class for performing queries related to <see cref="User"/>.
    /// </summary>
    public class AspNetUsersRepository : RepositoryBase<User>, IAspNetUsersRepository
    {
        /// <inheritdoc />
        public bool FindByNormalizedUserNameOrEmail(string normalizedValue_)
        {
            return dbSet.FirstOrDefault(user_ => user_.NormalizedUserName == normalizedValue_ || user_.NormalizedEmail == normalizedValue_) != null;
        }

        /// <inheritdoc />
        public IEnumerable<User> FindUsersHavingRoles(IEnumerable<string> roleNames_)
        {
            IEnumerable<string> normalizedRoleNames = roleNames_.Select(s_ => s_.ToUpperInvariant());

            // TODO IF APPLICABLE add another where clause "where u. ... == ..." to keep only active users
            // and rename current method to FindActiveUsersHavingRoles (update interface's summary)
            var users =
                from user in storageContext.Set<User>()
                join identityUserRole in storageContext.Set<IdentityUserRole<string>>() on user.Id equals identityUserRole.UserId
                join identityRole in storageContext.Set<IdentityRole<string>>() on identityUserRole.RoleId equals identityRole.Id
                where normalizedRoleNames.Contains(identityRole.NormalizedName)
                select user;

            return users.ToList();
        }
    }
}