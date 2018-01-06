// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public virtual Permission FindBy(string code_, string originExtensionAssemblyName_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Name == code_ && e_.OriginExtension == originExtensionAssemblyName_);
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

        public virtual void Delete(string entityId_)
        {
            var entity = dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
            if(entity != null)
                dbSet.Remove(entity);
        }

         public HashSet<Permission> AllForUser(string userId_)
        {
            IEnumerable<Permission> permissionsOfRoles = from p in storageContext.Set<Permission>()
                join rp in storageContext.Set<RolePermission>() on p.Id equals rp.PermissionId
                join r in storageContext.Set<Role>() on rp.RoleId equals r.Id
                join ur in storageContext.Set<IdentityUserRole<string>>() on r.Id equals ur.RoleId
                where ur.UserId == userId_
                select p;

            IEnumerable<Permission> permissionsOfGroups = from p in storageContext.Set<Permission>()
                join gp in storageContext.Set<GroupPermission>() on p.Id equals gp.PermissionId
                join g in storageContext.Set<Group>() on gp.GroupId equals g.Id
                join ug in storageContext.Set<UserGroup>() on g.Id equals ug.GroupId
                where ug.UserId == userId_
                select p;

            IEnumerable<Permission> permissionsOfUser = from p in storageContext.Set<Permission>()
                join up in storageContext.Set<UserPermission>() on p.Id equals up.PermissionId
                where up.UserId == userId_
                select p;

            HashSet<Permission> allPermissions = new HashSet<Permission>();
            foreach (Permission p in permissionsOfRoles)
            {
                if(!allPermissions.Contains(p))
                    allPermissions.Add(p);
            }
            foreach (Permission p in permissionsOfGroups)
            {
                if(!allPermissions.Contains(p))
                    allPermissions.Add(p);
            }
            foreach (Permission p in permissionsOfUser)
            {
                if(!allPermissions.Contains(p))
                    allPermissions.Add(p);
            }

            return allPermissions;
        }
    }
}
