// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using ExtCore.Data.EntityFramework;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class AspNetUsersRepository: RepositoryBase<User>, IAspNetUsersRepository
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
    }
}