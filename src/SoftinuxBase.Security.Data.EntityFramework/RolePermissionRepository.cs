// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SecurityTest")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SeedDatabase")]
namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class RolePermissionRepository : RepositoryBase<RolePermission>, IRolePermissionRepository
    {
        /// <inheritdoc />
        public IEnumerable<RolePermission> All()
        {
            return storageContext.Set<RolePermission>().ToList();
        }

        /// <inheritdoc />
        public IEnumerable<RolePermission> AllRolesWithPermissions()
        {
            var all = from rolePermission in storageContext.Set<RolePermission>()
                      join permission in storageContext.Set<Permission>() on rolePermission.PermissionId equals permission.Id
                      select new { RolePermission = rolePermission, Permission = permission };

            foreach (dynamic item in all)
            {
                item.RolePermission.Permission = item.Permission;
                yield return item.RolePermission;
            }
        }

        /// <inheritdoc />
        public RolePermission FindBy(string roleId_, string extensionName_)
        {
            return AllRolesWithPermissions().FirstOrDefault(e_ => e_.RoleId == roleId_ && e_.Extension == extensionName_);
        }

        /// <inheritdoc />
        public IEnumerable<RolePermission> FindBy(string extensionName_, Common.Enums.Permission level_)
        {
            var data = from rolePermisson in storageContext.Set<RolePermission>()
                   join permission in storageContext.Set<Permission>() on rolePermisson.PermissionId equals permission.Id
                   join identityRoles in storageContext.Set<IdentityRole<string>>() on rolePermisson.RoleId equals identityRoles.Id
                   where rolePermisson.Extension == extensionName_ && permission.Name == level_.GetPermissionName()
                   select new RolePermission
                   {
                       Extension = rolePermisson.Extension,
                       RoleId = rolePermisson.RoleId,
                       Id = rolePermisson.Id,
                       Role = new IdentityRole<string>(identityRoles.Name) { Id = identityRoles.Id, NormalizedName = identityRoles.NormalizedName },
                       PermissionId = rolePermisson.PermissionId
                   };

            return data.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<RolePermission> FilteredByRoleId(string roleId_)
        {
            return AllRolesWithPermissions().Where(e_ => e_.RoleId == roleId_).ToList();
        }

        /// <inheritdoc />
        public virtual void Create(RolePermission entity_)
        {
            dbSet.Add(entity_);
        }

        /// <inheritdoc />
        public virtual void Edit(RolePermission entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        /// <inheritdoc />
        public void Delete(string roleId_, string extensions_)
        {
            var entity = FindBy(roleId_, extensions_);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        /// <inheritdoc />
        public void DeleteAll()
        {
            dbSet.RemoveRange(dbSet.ToArray());
        }
    }
}