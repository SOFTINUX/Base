// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IGroupRepository : IRepository
    {
        Group WithKey(int entityId_);
        IEnumerable<Group> All();
        void Create(Group entity_);
        void Edit(Group entity_);
        void Delete(int entityId_);
    }
}
