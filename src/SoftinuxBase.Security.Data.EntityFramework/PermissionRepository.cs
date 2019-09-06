// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using Permission = SoftinuxBase.Security.Data.Entities.Permission;

// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SecurityTest")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("CommonTest")]
namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        /// <inheritdoc />
        public virtual IEnumerable<Permission> All()
        {
            return dbSet.ToList();
        }

        /// <inheritdoc />
        public virtual void Create(Permission entity_)
        {
            dbSet.Add(entity_);
        }

        /// <inheritdoc />
        public virtual void Edit(Permission entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        /// <inheritdoc />
        public virtual void Delete(string permissionId_)
        {
            var entity = dbSet.FirstOrDefault(permission_ => permission_.Id == permissionId_);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        /// <inheritdoc />
        public HashSet<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>> AllForUser(string userId_)
        {
            IEnumerable<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>> permissionsOfRoles =
                from permission in storageContext.Set<Permission>()
                join rolePermission in storageContext.Set<RolePermission>() on permission.Id equals rolePermission.PermissionId
                join identityRole in storageContext.Set<IdentityRole<string>>() on rolePermission.RoleId equals identityRole.Id
                join identityUserRole in storageContext.Set<IdentityUserRole<string>>() on identityRole.Id equals identityUserRole.RoleId
                where identityUserRole.UserId == userId_
                select new KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>(
                    SoftinuxBase.Security.Common.PermissionHelper.FromName(permission.Name), rolePermission.Extension);

            IEnumerable<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>> permissionsOfUser =
                from permission in storageContext.Set<Permission>()
                join userPermission in storageContext.Set<UserPermission>() on permission.Id equals userPermission.PermissionId
                where userPermission.UserId == userId_
                select new KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>(
                    SoftinuxBase.Security.Common.PermissionHelper.FromName(permission.Name), userPermission.Extension);

            HashSet<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>> allPermissions =
                new HashSet<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>>();

            foreach (KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string> item in permissionsOfRoles)
            {
                allPermissions.Add(item);
            }

            foreach (KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string> item in permissionsOfUser)
            {
                allPermissions.Add(item);
            }

            return allPermissions;
        }

        /// <inheritdoc />
        public Permission Find(Security.Common.Enums.Permission permissionLevel_)
        {
            return All().FirstOrDefault(permission_ => permission_.NormalizedName == permissionLevel_.ToString().ToUpperInvariant());
        }
    }
}