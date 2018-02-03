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
    public class UserGroupRepository : RepositoryBase<UserGroup>, IUserGroupRepository
    {
       public virtual IEnumerable<UserGroup> All()
        {
            return dbSet.ToList();
        }

        public UserGroup FindBy(string groupId_, string userId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.GroupId == groupId_ && e_.UserId == userId_);
        }

        public IEnumerable<UserGroup> FilteredByGroupId(string groupId_)
        {
            return dbSet.Where(e_ => e_.GroupId == groupId_).ToList();
        }

        public IEnumerable<UserGroup> FilteredByUserId(string userId_)
        {
            return dbSet.Where(e_ => e_.UserId == userId_).ToList();
        }

        public virtual void Create(UserGroup entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(UserGroup entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public void Delete(string groupId_, string userId_)
        {
            dbSet.Remove(FindBy(groupId_, userId_));
        }

    }
}
