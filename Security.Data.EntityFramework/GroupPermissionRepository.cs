// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class GroupPermissionRepository : RepositoryBase<GroupPermission>, IGroupPermissionRepository
    {
        public GroupPermission WithKeys(int groupId_, int permissionId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.GroupId == groupId_ && e_.PermissionId == permissionId_);
        }

        public IEnumerable<GroupPermission> FilteredByGroupId(int groupId_)
        {
            return dbSet.Where(e_ => e_.GroupId == groupId_).ToList();
        }

        public virtual void Create(GroupPermission entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(GroupPermission entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public void Delete(int groupId_, int permissionId_)
        {
            throw new System.NotImplementedException();
        }

      }
}
