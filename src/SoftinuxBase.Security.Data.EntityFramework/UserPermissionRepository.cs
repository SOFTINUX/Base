// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class UserPermissionRepository : RepositoryBase<UserPermission>, IUserPermissionRepository
    {
        public UserPermission FindBy(string userId_, string permissionId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.UserId == userId_ && e_.PermissionId == permissionId_);
        }

        public UserPermission FindBy(string extensionName_, Common.Enums.Permission level_)
        {
            IEnumerable<UserPermission> found = from up in storageContext.Set<UserPermission>()
                join p in storageContext.Set<Permission>() on up.PermissionId equals p.Id
                where up.Extension == extensionName_ && p.Name == level_.GetPermissionName()
                select up;
            return found.FirstOrDefault();
        }

        public IEnumerable<UserPermission> FilteredByUserId(string userId_)
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

        public void Delete(string userId_, string permissionId_)
        {
            var entity = FindBy(userId_, permissionId_);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public void DeleteAll()
        {
            dbSet.RemoveRange(dbSet.ToArray());
        }
      }
}
