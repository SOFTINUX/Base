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
    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public virtual Permission WithKey(int entityId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
        }

        public virtual IEnumerable<Permission> All()
        {
            return dbSet.ToList();
        }

        public virtual void Create(Permission entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(Permission entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public virtual void Delete(int entityId_)
        {
            dbSet.Remove(WithKey(entityId_));
        }

        public IEnumerable<PermissionValue> GetPermissionCodeAndLevelByRoleForUserId(int userId_)
        {
            return from p in storageContext.Set<Permission>()
                   join rp in storageContext.Set<RolePermission>() on p.Id equals rp.PermissionId
                   join r in storageContext.Set<Role>() on rp.RoleId equals r.Id
                   join ur in storageContext.Set<UserRole>() on r.Id equals ur.RoleId
                   join pl in storageContext.Set<PermissionLevel>() on rp.PermissionLevelId equals pl.Id
                   where ur.UserId == userId_
                   select new PermissionValue { UniqueId = rp.Permission.UniqueIdentifier, Level = pl.Value, AdministratorOwner = p.AdministratorOwner};
        }

        public IEnumerable<PermissionValue> GetPermissionCodeAndLevelByGroupForUserId(int userId_)
        {
            return from p in storageContext.Set<Permission>()
                   join gp in storageContext.Set<GroupPermission>() on p.Id equals gp.PermissionId
                   join g in storageContext.Set<Group>() on gp.GroupId equals g.Id
                   join gu in storageContext.Set<GroupUser>() on g.Id equals gu.GroupId
                   join pl in storageContext.Set<PermissionLevel>() on gp.PermissionLevelId equals pl.Id
                   where gu.UserId == userId_
                   select new PermissionValue { UniqueId = gp.Permission.UniqueIdentifier, Level = pl.Value, AdministratorOwner = p.AdministratorOwner };
        }

        public IEnumerable<PermissionValue> GetPermissionCodeAndLevelByUserId(int userId_)
        {
            return from p in storageContext.Set<Permission>()
                   join up in storageContext.Set<UserPermission>() on p.Id equals up.PermissionId
                   join pl in storageContext.Set<PermissionLevel>() on up.PermissionLevelId equals pl.Id
                   where up.UserId == userId_
                   select new PermissionValue { UniqueId = up.Permission.UniqueIdentifier, Level = pl.Value, AdministratorOwner = p.AdministratorOwner };
        }
    }
}
