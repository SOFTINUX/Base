// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Permission = Security.Data.Entities.Permission;

namespace Security.Data.EntityFramework
{
    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
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
            if (entity != null)
                dbSet.Remove(entity);
        }

        /// <summary>
        /// Every permission with its scope, linked to user, user's groups, user's roles.
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns>List of key/value : permission and scope</returns>
        public HashSet<KeyValuePair<Infrastructure.Enums.Permission, string>> AllForUser(string userId_)
        {
            IEnumerable<KeyValuePair<Infrastructure.Enums.Permission, string>> permissionsOfRoles = from p in storageContext.Set<Permission>()
                                                                           join rp in storageContext.Set<RolePermission>() on p.Id equals rp.PermissionId
                                                                           join r in storageContext.Set<IdentityRole<string>>() on rp.RoleId equals r.Id
                                                                           join ur in storageContext.Set<IdentityUserRole<string>>() on r.Id equals ur.RoleId
                                                                           where ur.UserId == userId_
                                                                           select new KeyValuePair<Infrastructure.Enums.Permission, string>(PermissionHelper.FromId(p.Id), rp.Scope);

            IEnumerable<KeyValuePair<Infrastructure.Enums.Permission, string>> permissionsOfGroups = from p in storageContext.Set<Permission>()
                                                                            join gp in storageContext.Set<GroupPermission>() on p.Id equals gp.PermissionId
                                                                            join g in storageContext.Set<Group>() on gp.GroupId equals g.Id
                                                                            join ug in storageContext.Set<UserGroup>() on g.Id equals ug.GroupId
                                                                            where ug.UserId == userId_
                                                                            select new KeyValuePair<Infrastructure.Enums.Permission, string>(PermissionHelper.FromId(p.Id), gp.Scope);

            IEnumerable<KeyValuePair<Infrastructure.Enums.Permission, string>> permissionsOfUser = from p in storageContext.Set<Permission>()
                                                                          join up in storageContext.Set<UserPermission>() on p.Id equals up.PermissionId
                                                                          where up.UserId == userId_
                                                                          select new KeyValuePair<Infrastructure.Enums.Permission, string>(PermissionHelper.FromId(p.Id), up.Scope);

            HashSet<KeyValuePair<Infrastructure.Enums.Permission, string>> allPermissions = new HashSet<KeyValuePair<Infrastructure.Enums.Permission, string>>();

            foreach (KeyValuePair<Infrastructure.Enums.Permission, string> p in permissionsOfRoles)
                allPermissions.Add(p);

            foreach (KeyValuePair<Infrastructure.Enums.Permission, string> p in permissionsOfGroups)
                allPermissions.Add(p);

            foreach (KeyValuePair<Infrastructure.Enums.Permission, string> p in permissionsOfUser)
                allPermissions.Add(p);


            return allPermissions;
        }
    }
}
