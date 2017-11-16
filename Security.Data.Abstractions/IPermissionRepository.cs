// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IPermissionRepository : IRepository
    {
        Permission WithKey(int entityId_);

        /// <summary>
        /// Finds a permission by code and origin extension assembly "short name" (Assembly.GetName().Name).
        /// </summary>
        /// <param name="code_"></param>
        /// <param name="assemblyName_"></param>
        /// <returns></returns>
        Permission WithKeys(string code_, string originExtensionAssemblyName_);
        IEnumerable<Permission> All();
        void Create(Permission entity_);
        void Edit(Permission entity_);
        void Delete(int entityId_);
        IEnumerable<PermissionValue> GetPermissionCodeAndLevelByRoleForUserId(int userId_);
        IEnumerable<PermissionValue> GetPermissionCodeAndLevelByGroupForUserId(int userId_);
        IEnumerable<PermissionValue> GetPermissionCodeAndLevelByUserId(int userId_);
    }
}
