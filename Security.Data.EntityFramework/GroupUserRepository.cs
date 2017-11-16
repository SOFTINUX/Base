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
    public class GroupUserRepository : RepositoryBase<GroupUser>, IGroupUserRepository
    {
       public virtual IEnumerable<GroupUser> All()
        {
            return dbSet.ToList();
        }

        public GroupUser WithKeys(int groupId_, int userId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.GroupId == groupId_ && e_.UserId == userId_);
        }

        public IEnumerable<GroupUser> FilteredByGroupId(int groupId_)
        {
            return dbSet.Where(e_ => e_.GroupId == groupId_).ToList();
        }

        public IEnumerable<GroupUser> FilteredByUserId(int userId_)
        {
            return dbSet.Where(e_ => e_.UserId == userId_).ToList();
        }

        public virtual void Create(GroupUser entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(GroupUser entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public void Delete(int groupId_, int userId_)
        {
            dbSet.Remove(WithKeys(groupId_, userId_));
        }

    }
}
