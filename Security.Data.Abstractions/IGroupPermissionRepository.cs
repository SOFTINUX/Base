// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IGroupPermissionRepository : IRepository
    {
        GroupPermission FindBy(string groupId_, string permissionId_);
        IEnumerable<GroupPermission> FilteredByGroupId(string groupId_);
        void Create(GroupPermission entity_);
        void Edit(GroupPermission entity_);
        void Delete(string groupId_, string permissionId_);
    }
}
