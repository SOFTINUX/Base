// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public virtual Group WithKey(int entityId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
        }

        public virtual Group WithKeys(string code_, string originExtensionAssemblyName_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Code == code_ && e_.OriginExtension == originExtensionAssemblyName_);
        }

        public virtual IEnumerable<Group> All()
        {
            return dbSet.ToList();
        }

        public virtual void Create(Group entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(Group entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public virtual void Delete(int entityId_)
        {
            dbSet.Remove(WithKey(entityId_));
        }
    }
}
