// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security.Data.Abstractions
{
    public interface IPermissionRepository : IRepository
    {
        IEnumerable<Permission> All();
        void Create(Permission entity_);
        void Edit(Permission entity_);
        void Delete(string entityId_);

        /// <summary>
        /// Every permission with its extension, linked to user and user's roles.
        /// </summary>
        /// <param name="userId_">user ID.</param>
        /// <returns>Set of permission levels associated to extensions.</returns>
        HashSet<KeyValuePair<SoftinuxBase.Security.Common.Enums.Permission, string>> AllForUser(string userId_);

        /// <summary>
        /// Find a permission by enum value.
        /// </summary>
        /// <param name="permissionLevel_">Permission level.</param>
        /// <returns>Permission entity.</returns>
        Permission Find(Security.Common.Enums.Permission permissionLevel_);
    }
}
