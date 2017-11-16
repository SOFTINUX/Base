// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IGroupUserRepository : IRepository
    {
        GroupUser WithKeys(int groupId_, int userId_);
        IEnumerable<GroupUser> FilteredByGroupId(int groupId_);
        IEnumerable<GroupUser> FilteredByUserId(int userId_);
        void Create(GroupUser entity_);
        void Edit(GroupUser entity_);
        void Delete(int groupId_, int userId_);
    }
}
