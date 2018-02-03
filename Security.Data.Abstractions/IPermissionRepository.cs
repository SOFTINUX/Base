// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IPermissionRepository : IRepository
    {
        /// <summary>
        /// Finds a permission by name and origin extension assembly "short name" (Assembly.GetName().Name).
        /// </summary>
        /// <param name="name_">Permission name. Base names in Infrastructure.Enums.Permissions</param>
        /// <param name="originExtensionAssemblyName_">extension assembly "short name" (Assembly.GetName().Name)</param>
        /// <returns></returns>
        Permission FindBy(string name_, string originExtensionAssemblyName_);
        IEnumerable<Permission> All();
        void Create(Permission entity_);
        void Edit(Permission entity_);
        void Delete(string entityId_);
        /// <summary>
        /// All permissions linked to user, user's groups, user's roles.
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns></returns>
        HashSet<Permission> AllForUser(string userId_);
    }
}
