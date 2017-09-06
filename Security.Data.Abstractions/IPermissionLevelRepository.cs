using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IPermissionLevelRepository : IRepository
    {
        PermissionLevel WithKey(int entityId_);
        IEnumerable<PermissionLevel> All();
    }
}
