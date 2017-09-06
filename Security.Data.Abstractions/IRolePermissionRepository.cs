using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IRolePermissionRepository : IRepository
    {
        RolePermission WithKeys(int roleId_, int permissionId_);
        IEnumerable<RolePermission> FilteredByRoleId(int roleId_);
        void Create(RolePermission entity_);
        void Edit(RolePermission entity_);
        void Delete(int roleId_, int permissionId_);
    }
}
