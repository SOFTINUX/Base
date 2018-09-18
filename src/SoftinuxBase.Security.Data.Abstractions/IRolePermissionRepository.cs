// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IRolePermissionRepository : IRepository
    {
        List<RolePermission> All();
        RolePermission FindBy(string roleId_, string scope_);
        IEnumerable<RolePermission> FilteredByRoleId(string roleId_);
        void Create(RolePermission entity_);
        void Edit(RolePermission entity_);
        void Delete(string roleId_, string scope_);
        void DeleteAll();
    }
}
