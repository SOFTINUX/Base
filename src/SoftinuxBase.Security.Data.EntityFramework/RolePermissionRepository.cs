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

namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class RolePermissionRepository : RepositoryBase<RolePermission>, IRolePermissionRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Return a <see cref="IEnumerable" /> of <see cref="RolePermission" />.</returns>
        public IEnumerable<RolePermission> All()
        {
            IEnumerable all = from rp in storageContext.Set<RolePermission>()
                              join p in storageContext.Set<Permission>() on rp.PermissionId equals p.Id
                              select new { RolePermission = rp, Permission = p };

            foreach (dynamic item in all)
            {
                item.RolePermission.Permission = item.Permission;
                yield return item.RolePermission;
            }
        }

        /// <summary>
        /// Find role and his attached extensions
        /// </summary>
        /// <param name="roleId_">The role Id as <see cref="string" />.</param>
        /// <param name="extensionName_">The extension name as <see cref="string" />.</param>
        /// <returns></returns>
        public RolePermission FindBy(string roleId_, string extensionName_)
        {
            return All().FirstOrDefault(e_ => e_.RoleId == roleId_ && e_.Extension == extensionName_);
        }

        /// <summary>
        /// Finds records in RolePermission table that matches the parameter extension name and permission level.
        /// Additionally retrieve the role name.
        /// </summary>
        /// <param name="extensionName_">Name of extension.</param>
        /// <param name="level_">Permiossion level.</param>
        /// <returns>The RolePermission objects with associated Role object.</returns>
        public IEnumerable<RolePermission> FindBy(string extensionName_, Common.Enums.Permission level_)
        {
            // TODO write the query with the other query syntax? (fluent)
            return from rp in storageContext.Set<RolePermission>()
                   join p in storageContext.Set<Permission>() on rp.PermissionId equals p.Id
                   join r in storageContext.Set<IdentityRole<string>>() on rp.RoleId equals r.Id
                   where rp.Extension == extensionName_ && p.Name == level_.GetPermissionName()
                   select new RolePermission
                   {
                       Extension = rp.Extension,
                       RoleId = rp.RoleId,
                       Id = rp.Id,
                       Role = new IdentityRole<string>(r.Name) { Id = r.Id, NormalizedName = r.NormalizedName },
                       PermissionId = rp.PermissionId
                   };
        }

        /// <summary>
        ///  Return filtered roles list.
        /// </summary>
        /// <param name="roleId_">role id.</param>
        /// <returns>Return a <see cref="IEnumerable" /> of <see cref="RolePermission" />.</returns>
        public IEnumerable<RolePermission> FilteredByRoleId(string roleId_)
        {
            return All().Where(e_ => e_.RoleId == roleId_).ToList();
        }

        /// <summary>
        /// Create a Role Permission record
        /// </summary>
        /// <param name="entity_"><see cref="RolePermission" />.</param>
        public virtual void Create(RolePermission entity_)
        {
            dbSet.Add(entity_);
        }

        /// <summary>
        /// Modify a Role Permission field
        /// </summary>
        /// <param name="entity_"><see cref="RolePermission" />.</param>
        public virtual void Edit(RolePermission entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="roleId_"></param>
        /// <param name="extensions_"></param>
        public void Delete(string roleId_, string extensions_)
        {
            var entity = FindBy(roleId_, extensions_);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        /// <summary>
        /// Delete All
        /// </summary>
        public void DeleteAll()
        {
            dbSet.RemoveRange(dbSet.ToArray());
        }
    }
}