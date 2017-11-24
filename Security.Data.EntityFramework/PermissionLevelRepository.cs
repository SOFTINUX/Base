// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Infrastructure.Enums;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class PermissionLevelRepository : RepositoryBase<PermissionLevel>, IPermissionLevelRepository
    {
        public void Create(PermissionLevel entity_)
        {
            dbSet.Add(entity_);
        }

        /// <summary>
        /// Loads a record by matching on "value" column.
        /// Permission level values are a list of frozen values.
        /// </summary>
        /// <param name="levelValue_"></param>
        /// <returns></returns>
        public virtual PermissionLevel ByValue(PermissionLevelValue levelValue_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Value == (byte) levelValue_);
        }

        public virtual IEnumerable<PermissionLevel> All()
        {
            return dbSet.ToList();
        }
    }
}