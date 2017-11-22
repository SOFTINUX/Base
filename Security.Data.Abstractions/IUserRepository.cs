// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IUserRepository : IRepository
    {
        User WithKey(int entityId_);
        User WithKeys(string firstName_, string lastName_, string displayName_);
        User WithCredentialIdentifier(string identifier_);
        IEnumerable<User> All();
        void Create(User entity_);
        void Edit(User entity_);
        void Delete(int entityId_);
    }
}
