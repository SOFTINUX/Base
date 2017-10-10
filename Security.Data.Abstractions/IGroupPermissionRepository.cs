using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IGroupPermissionRepository : IRepository
    {
        GroupPermission WithKeys(int groupId_, int permissionId_);
        IEnumerable<GroupPermission> FilteredByGroupId(int groupId_);
        void Create(GroupPermission entity_);
        void Edit(GroupPermission entity_);
        void Delete(int groupId_, int permissionId_);
    }
}
