using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {

 
        public UserRole WithKeys(int userId_, int roleId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.UserId == userId_ && e_.RoleId == roleId_);
        }

        public IEnumerable<UserRole> FilteredByRoleId(int roleId_)
        {
            return dbSet.Where(e_ => e_.RoleId == roleId_).ToList();
        }

        public IEnumerable<UserRole> FilteredByUserId(int userId_)
        {
            return dbSet.Where(e_ => e_.UserId == userId_).ToList();
        }

        public virtual void Create(UserRole entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(UserRole entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public void Delete(int userId_, int roleId_)
        {
            dbSet.Remove(WithKeys(userId_, roleId_));
        }

    }
}
