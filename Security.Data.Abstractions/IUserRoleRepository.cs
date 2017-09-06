using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IUserRoleRepository : IRepository
    {
        UserRole WithKeys(int userId_, int roleId_);
        IEnumerable<UserRole> FilteredByRoleId(int roleId_);
        IEnumerable<UserRole> FilteredByUserd(int userId_);
        void Create(UserRole entity_);
        void Edit(UserRole entity_);
        void Delete(int userId_, int roleId_);
    }
}
