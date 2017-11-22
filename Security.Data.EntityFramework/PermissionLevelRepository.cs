// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
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

        public virtual PermissionLevel WithKey(int entityId_)
       {
           return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
       }

        public PermissionLevel WithValue(int value_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Id == value_);
        }

        public virtual IEnumerable<PermissionLevel> All()
        {
            return dbSet.ToList();
        }

    }
}
