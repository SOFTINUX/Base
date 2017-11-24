// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure.Enums;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IPermissionLevelRepository : IRepository
    {
        void Create(PermissionLevel entity_);
        PermissionLevel ByValue(PermissionLevelValue levelValue_);
        IEnumerable<PermissionLevel> All();
    }
}
