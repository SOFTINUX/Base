using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IPermissionLevelRepository : IRepository
    {
        void Create(PermissionLevel entity_);
        PermissionLevel WithKey(int entityId_);
        IEnumerable<PermissionLevel> All();
    }
}
