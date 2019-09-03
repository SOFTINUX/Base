// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.EntityFramework")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SeedDatabase")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("CommonTest")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SecurityTest")]
namespace SoftinuxBase.Security.Data.Abstractions
{
    /// <summary>
    /// An interface for performing queries related to <see cref="Permission"/>.
    /// </summary>
    internal interface IPermissionRepository : IRepository
    {
        /// <summary>
        /// Read all the permissions.
        /// </summary>
        /// <returns><see cref="Permission"/>.</returns>
        IEnumerable<Permission> All();

        /// <summary>
        /// Create a new <see cref="Permission"/>.
        /// </summary>
        /// <param name="entity_">the <see cref="Permission"/> to persist.</param>
        void Create(Permission entity_);

        /// <summary>
        /// Update a <see cref="Permission"/>.
        /// </summary>
        /// <param name="entity_">the modified <see cref="Permission"/> to persist.</param>
        void Edit(Permission entity_);

        /// <summary>
        /// Delete a <see cref="Permission"/>.
        /// </summary>
        /// <param name="permissionId_">the ID of the <see cref="Permission"/> to delete.</param>
        void Delete(string permissionId_);

        /// <summary>
        /// Read the permissions levels for every extension, related to an <see cref="User"/>.
        /// Permission levels are inherited from user's roles and user own permissions.
        /// User's own permission levels have highest priority to define effective permission levels.
        /// </summary>
        /// <param name="userId_">user ID.</param>
        /// <returns>Set of <see cref="SoftinuxBase.Security.Common.Enums.Permission"/> associated to extensions.</returns>
        HashSet<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>> AllForUser(string userId_);

        /// <summary>
        /// Find a <see cref="Permission"/> by <see cref="Common.Enums.Permission"/> value.
        /// </summary>
        /// <param name="permissionLevel_">Permission level.</param>
        /// <returns><see cref="Permission"/>.</returns>
        Permission Find(Security.Common.Enums.Permission permissionLevel_);
    }
}
