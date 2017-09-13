using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class PermissionLevelRepository : RepositoryBase<PermissionLevel>, IPermissionLevelRepository
    {
       public virtual PermissionLevel WithKey(int entityId_)
       {
           return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
       }

        public virtual IEnumerable<PermissionLevel> All()
        {
            return dbSet.ToList();
        }

    }
}
