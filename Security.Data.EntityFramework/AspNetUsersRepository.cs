// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class AspNetUsersRepository: RepositoryBase<User>, IAspNetUsersRepository
    {
        /// <summary>
        /// Return true if find or false if null (not find)
        /// </summary>
        /// <param name="value_">string to find</param>
        /// <returns>bool</returns>
        public bool FindByUserNameOrEmail(string value_)
        {
            return dbSet.FirstOrDefault(e_ => e_.UserName == value_ || e_.Email == value_) != null;
        }
    }
}