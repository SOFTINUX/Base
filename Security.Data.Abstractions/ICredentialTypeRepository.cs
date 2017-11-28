// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface ICredentialTypeRepository : IRepository
    {
        void Create(CredentialType entity_);
        IEnumerable<CredentialType> All();
        /// <summary>
        ///
        /// </summary>
        /// <param name="code_">Code given from Infrastructure.Enums.CredentialTypes partial class or its other parts</param>
        /// <param name="originExtensionAssemblyName_"></param>
        /// <returns></returns>
        CredentialType FindBy(string code_, string originExtensionAssemblyName_); // TODO create and use Infrastructure.Enums.CredentialTypes partial class
    }
}
