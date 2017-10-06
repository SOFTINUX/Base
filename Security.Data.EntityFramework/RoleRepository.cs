using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
       public virtual Role WithKey(int entityId_)
       {
           return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
       }

        public virtual IEnumerable<Role> All()
        {
            return dbSet.ToList();
        }

        public virtual void Create(Role entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(Role entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public virtual void Delete(int entityId_)
        {
            dbSet.Remove(WithKey(entityId_));
        }

    }
}
