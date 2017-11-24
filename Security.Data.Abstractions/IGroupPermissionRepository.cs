// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IGroupPermissionRepository : IRepository
    {
        GroupPermission FindBy(int groupId_, int permissionId_);
        IEnumerable<GroupPermission> FilteredByGroupId(int groupId_);
        void Create(GroupPermission entity_);
        void Edit(GroupPermission entity_);
        void Delete(int groupId_, int permissionId_);
    }
}
