using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IGroupRepository : IRepository
    {
        Group WithKey(int entityId_);
        IEnumerable<Group> All();
        void Create(Group entity_);
        void Edit(Group entity_);
        void Delete(int entityId_);
    }
}
