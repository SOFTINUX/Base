// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IUserRoleRepository : IRepository
    {
        UserRole FindBy(int userId_, int roleId_);
        IEnumerable<UserRole> FilteredByRoleId(int roleId_);
        IEnumerable<UserRole> FilteredByUserId(int userId_);
        void Create(UserRole entity_);
        void Edit(UserRole entity_);
        void Delete(int userId_, int roleId_);
    }
}
