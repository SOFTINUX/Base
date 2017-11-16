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
    public class UserPermissionRepository : RepositoryBase<UserPermission>, IUserPermissionRepository
    {

        public UserPermission WithKeys(int userId_, int permissionId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.UserId == userId_ && e_.PermissionId == permissionId_);
        }

        public IEnumerable<UserPermission> FilteredByUserId(int userId_)
        {
            return dbSet.Where(e_ => e_.UserId == userId_).ToList();
        }

        public virtual void Create(UserPermission entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(UserPermission entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public void Delete(int userId_, int permissionId_)
        {
            throw new System.NotImplementedException();
        }

      }
}
