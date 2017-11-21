// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IGroupRepository : IRepository
    {
        Group WithKey(int entityId_);

        /// <summary>
        /// Finds a group by code and origin extension assembly "short name" (Assembly.GetName().Name).
        /// </summary>
        /// <param name="code_"></param>
        /// <param name="originExtensionAssemblyName_"></param>
        /// <returns></returns>
        Group WithKeys(string code_, string originExtensionAssemblyName_);
        IEnumerable<Group> All();
        void Create(Group entity_);
        void Edit(Group entity_);
        void Delete(int entityId_);
    }
}
