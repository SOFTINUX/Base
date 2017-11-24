// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class CredentialTypeRepository : RepositoryBase<CredentialType>, ICredentialTypeRepository
    {
        public void Create(CredentialType entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual IEnumerable<CredentialType> All()
        {
            return dbSet.ToList();
        }

        public virtual CredentialType FindBy(string code_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Code == code_);
        }
    }
}
