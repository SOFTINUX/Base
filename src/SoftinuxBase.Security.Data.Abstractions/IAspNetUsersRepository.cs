// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;

namespace SoftinuxBase.Security.Data.Abstractions
{
    public interface IAspNetUsersRepository : IRepository
    {
        bool FindByNormalizedUserNameOrEmail(string value_);
    }
}