// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IUserPermissionRepository : IRepository
    {
        UserPermission WithKeys(int userId_, int permissionId_);
        IEnumerable<UserPermission> FilteredByUserId(int userId_);
        void Create(UserPermission entity_);
        void Edit(UserPermission entity_);
        void Delete(int userId_, int permissionId_);
    }
}
