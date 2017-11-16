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
        CredentialType WithKey(int entityId_);
        IEnumerable<CredentialType> All();
        CredentialType WithCode(string code_);
    }
}
