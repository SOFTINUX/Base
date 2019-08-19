// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security.Data.Abstractions
{
    public interface IAspNetUsersRepository : IRepository
    {
        bool FindByNormalizedUserNameOrEmail(string value_);

        IEnumerable<User> FindActiveUsersHavingRole(string roleName_);
    }
}