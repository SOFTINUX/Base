using System;
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

        public IEnumerable<Tuple<string, int>> GetPermissionCodeAndLevelByRoleForUserId(int userId_)
        {
            IEnumerable<RolePermission> perms = from p in storageContext.Set<Permission>()
                join rp in storageContext.Set<RolePermission>() on p.Id equals rp.PermissionId
                join r in storageContext.Set<Role>() on rp.RoleId equals r.Id
                join ur in storageContext.Set<UserRole>() on r.Id equals ur.RoleId
                where ur.UserId == userId_
                select rp;
            IEnumerable<Tuple<string, int>> values = new List<Tuple<string, int>>();
            foreach (RolePermission rp in perms)
            {
                values.Append(new Tuple<string, int>(rp.Permission.UniqueIdentifier, rp.PermissionLevelId));
            }
            return values;
        }

        public IEnumerable<Tuple<string, int>> GetPermissionCodeAndLevelByGroupForUserId(int userId_)
        {
            IEnumerable<GroupPermission> perms = from p in storageContext.Set<Permission>()
                join gp in storageContext.Set<GroupPermission>() on p.Id equals gp.PermissionId
                join g in storageContext.Set<Group>() on gp.GroupId equals g.Id
                join gu in storageContext.Set<GroupUser>() on g.Id equals gu.GroupId
                where gu.UserId == userId_
                select gp;

            IEnumerable<Tuple<string, int>> values = new List<Tuple<string, int>>();
            foreach (GroupPermission rp in perms)
            {
                values.Append(new Tuple<string, int>(rp.Permission.UniqueIdentifier, rp.PermissionLevelId));
            }
            return values;
        }

        public IEnumerable<Tuple<string, int>> GetPermissionCodeAndLevelByUserId(int userId_)
        {
            IEnumerable<UserPermission> perms = from p in storageContext.Set<Permission>()
                join up in storageContext.Set<UserPermission>() on p.Id equals up.PermissionId
                where up.UserId == userId_
                select up;

            IEnumerable<Tuple<string, int>> values = new List<Tuple<string, int>>();
            foreach (UserPermission rp in perms)
            {
                values.Append(new Tuple<string, int>(rp.Permission.UniqueIdentifier, rp.PermissionLevelId));
            }
            return values;
        }
    }
}
