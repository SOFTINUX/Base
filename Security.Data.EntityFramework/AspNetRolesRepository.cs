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
    public class AspNetRolesRepository: RepositoryBase<User>, IAspNetRolesRepository
    {
        /// <summary>
        /// Return true if found, else false.
        /// </summary>
        /// <param name="normalizedValue_">string to find</param>
        /// <returns>bool</returns>
        public bool FindByNormalizedName(string normalizedValue_)
        {
            return dbSet.FirstOrDefault(e_ => e_.NormalizedUserName == normalizedValue_) != null;
        }
    }
}