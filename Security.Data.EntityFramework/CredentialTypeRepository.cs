using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class CredentialTypeRepository : RepositoryBase<CredentialType>, ICredentialTypeRepository
    {
       public virtual CredentialType WithKey(int entityId_)
       {
           return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
       }

        public virtual IEnumerable<CredentialType> All()
        {
            return dbSet.ToList();
        }

        public virtual void Create(CredentialType entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(CredentialType entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public virtual void Delete(int entityId_)
        {
            dbSet.Remove(WithKey(entityId_));
        }
    }
}
