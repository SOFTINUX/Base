// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.EntityFramework")]
namespace SoftinuxBase.Security.Data.Abstractions
{
    public interface IAspNetUsersRepository : IRepository
    {
        bool FindByNormalizedUserNameOrEmail(string normalizedValue_);

        IEnumerable<User> FindActiveUsersHavingRoles(IEnumerable<string> roleNames_);
    }
}