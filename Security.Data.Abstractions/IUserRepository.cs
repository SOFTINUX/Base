using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IUserRepository : IRepository
    {
        User WithKey(int entityId_);
        User WithCredentialIdentifier(string identifier_);
        IEnumerable<User> All();
        void Create(User entity_);
        void Edit(User entity_);
        void Delete(int entityId_);
    }
}
