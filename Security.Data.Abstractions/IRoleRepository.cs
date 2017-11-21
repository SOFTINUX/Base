// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Common.Enums;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IRoleRepository : IRepository
    {
        Role WithKey(int entityId_);
        Role WithKey(RoleId roleId_);

        /// <summary>
        /// Finds a role by code and origin extension assembly "short name" (Assembly.GetName().Name).
        /// </summary>
        /// <param name="code_"></param>
        /// <param name="originExtensionAssemblyName_"></param>
        /// <returns></returns>
        Role WithKeys(string code_, string originExtensionAssemblyName_);
        IEnumerable<Role> All();
        void Create(Role entity_);
        void Edit(Role entity_);
        void Delete(int entityId_);
    }
}
