// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.Data.Entities;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.EntityFramework")]
namespace SoftinuxBase.Security.Data.Abstractions
{
    /// <summary>
    /// An interface for performing queries related to <see cref="User"/>.
    /// </summary>
    internal interface IAspNetUsersRepository : IRepository
    {
        /// <summary>
        /// Check by name or e-mail user existence.
        /// </summary>
        /// <param name="normalizedValue_">String value from <see cref="UserManager{TUser}.NormalizeKey"/>.</param>
        /// <returns>A bool indicating that an user was found.</returns>
        bool FindByNormalizedUserNameOrEmail(string normalizedValue_);

        /// <summary>
        /// Find all the users linked to roles.
        /// </summary>
        /// <param name="roleNames_"><see cref="IEnumerable{String}"/> role names.</param>
        /// <returns><see cref="IEnumerable{User}"/> users.</returns>
        IEnumerable<User> FindUsersHavingRoles(IEnumerable<string> roleNames_);
    }
}