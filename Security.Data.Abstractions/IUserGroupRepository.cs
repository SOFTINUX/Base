// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IUserGroupRepository : IRepository
    {
        UserGroup FindBy(string groupId_, string userId_);
        IEnumerable<UserGroup> FilteredByGroupId(string groupId_);
        IEnumerable<UserGroup> FilteredByUserId(string userId_);
        void Create(UserGroup entity_);
        void Edit(UserGroup entity_);
        void Delete(string groupId_, string userId_);
    }
}
