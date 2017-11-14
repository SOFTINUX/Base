// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Common.Enums;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
       public virtual Role WithKey(int entityId_)
       {
           return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
       }

       public virtual Role WithKey(RoleId roleId_) {
           return WithKey((int) roleId_);
       }

        public virtual IEnumerable<Role> All()
        {
            return dbSet.ToList();
        }

        public virtual void Create(Role entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(Role entity_)
        {
            ((DbContext)storageContext).Entry(entity_).State = EntityState.Modified;
        }

        public virtual void Delete(int entityId_)
        {
            dbSet.Remove(WithKey(entityId_));
        }

    }
}
