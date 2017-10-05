using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IRoleRepository : IRepository
    {
        Role WithKey(int entityId_);
        IEnumerable<Role> All();
        void Create(Role entity_);
        void Edit(Role entity_);
        void Delete(int entityId_);

        IEnumerable<Role> FilterByUserId(int userId_);
    }
}
