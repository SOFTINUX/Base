// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;

namespace Security.Data.Abstractions
{
    public interface IAspNetUsersRepository: IRepository
    {
        bool FindByUserNameOrEmail(string value_);
    }
}