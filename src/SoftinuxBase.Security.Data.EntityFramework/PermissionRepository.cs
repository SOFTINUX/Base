// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using Permission = SoftinuxBase.Security.Data.Entities.Permission;

namespace SoftinuxBase.Security.Data.EntityFramework
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
        /// Every permission with its scope, linked to user and user's roles.
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns>List of key/value : permission and scope</returns>
        public HashSet<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>> AllForUser(string userId_)
        {
            IEnumerable<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>> permissionsOfRoles = from p in storageContext.Set<Permission>()
                                                                           join rp in storageContext.Set<RolePermission>() on p.Id equals rp.PermissionId
                                                                           join r in storageContext.Set<IdentityRole<string>>() on rp.RoleId equals r.Id
                                                                           join ur in storageContext.Set<IdentityUserRole<string>>() on r.Id equals ur.RoleId
                                                                           where ur.UserId == userId_
                                                                           select new KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>(SoftinuxBase.Security.Common.PermissionHelper.FromName(p.Name), rp.Extension);

            IEnumerable<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>> permissionsOfUser = from p in storageContext.Set<Permission>()
                                                                          join up in storageContext.Set<UserPermission>() on p.Id equals up.PermissionId
                                                                          where up.UserId == userId_
                                                                          select new KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>(SoftinuxBase.Security.Common.PermissionHelper.FromName(p.Name), up.Extension);

            HashSet<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>> allPermissions = new HashSet<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>>();

            foreach (KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string> p in permissionsOfRoles)
                allPermissions.Add(p);

            foreach (KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string> p in permissionsOfUser)
                allPermissions.Add(p);


            return allPermissions;
        }

        /// <summary>
        /// Find a permission by enum value.
        /// </summary>
        /// <param name="permissionValue_"></param>
        /// <returns></returns>
        public Permission Find(Security.Common.Enums.Permission permissionValue_)
        {
            return All().FirstOrDefault(p_ => p_.NormalizedName == permissionValue_.ToString().ToUpperInvariant());
        }
    }
}
