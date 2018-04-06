// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IPermissionRepository : IRepository
    {
        IEnumerable<Permission> All();
        void Create(Permission entity_);
        void Edit(Permission entity_);
        void Delete(string entityId_);
        /// <summary>
        /// Every permission with its scope, linked to user, user's roles.
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns></returns>
        HashSet<KeyValuePair<Common.Enums.Permission, string>> AllForUser(string userId_);
    }
}
