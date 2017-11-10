// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IRoleRepository : IRepository
    {
        Role WithKey(int entityId_);
        IEnumerable<Role> All();
        void Create(Role entity_);
        void Edit(Role entity_);
        void Delete(int entityId_);
    }
}
