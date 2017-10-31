using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class RolePermissionRepository : RepositoryBase<RolePermission>, IRolePermissionRepository
    {

        public RolePermission WithKeys(int roleId_, int permissionId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.RoleId == roleId_ && e_.PermissionId == permissionId_);
        }

        public IEnumerable<RolePermission> FilteredByRoleId(int roleId_)
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

        public void Delete(int roleId_, int permissionId_)
        {
            throw new System.NotImplementedException();
        }

     }
}
