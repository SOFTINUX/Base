// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class RolePermissionRepository : RepositoryBase<RolePermission>, IRolePermissionRepository
    {

        public List<RolePermission> All()
        {
            return dbSet.ToList();
        }
        public RolePermission FindBy(string roleId_, string scope_)
        {
            return dbSet.FirstOrDefault(e_ => e_.RoleId == roleId_ && e_.Scope == scope_);
        }

        public IEnumerable<RolePermission> FilteredByRoleId(string roleId_)
        {
            return dbSet.Where(e_ => e_.RoleId == roleId_).ToList();
        }

        public virtual void Create(RolePermission entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(RolePermission entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public void Delete(string roleId_, string scope_)
        {
            var entity = FindBy(roleId_, scope_);
            if (entity != null)
                dbSet.Remove(entity);
        }

        public void DeleteAll()
        {
            dbSet.RemoveRange(dbSet.ToArray());
        }

     }
}
