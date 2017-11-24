// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface ICredentialRepository : IRepository
    {
        Credential FindBy(int credentialTypeId_, string identifier_);
        IEnumerable<Credential> All();
        void Create(Credential entity_);
        void Edit(Credential entity_);
        void Delete(int entityId_);

    }
}
