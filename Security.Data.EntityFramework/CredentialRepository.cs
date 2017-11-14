// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class CredentialRepository : RepositoryBase<Credential>, ICredentialRepository
    {
       public virtual Credential WithKey(int entityId_)
       {
           return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
       }

        public virtual Credential WithKeys(int credentialTypeId_, string identifier_)
        {
            return dbSet.FirstOrDefault(e_ => e_.CredentialTypeId == credentialTypeId_ && e_.Identifier == identifier_);
        }

        public virtual IEnumerable<Credential> All()
        {
            return dbSet.ToList();
        }

        public virtual void Create(Credential entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(Credential entity_)
        {
            ((DbContext) storageContext).Entry(entity_).State = EntityState.Modified;
        }

        public virtual void Delete(int entityId_)
        {
            dbSet.Remove(WithKey(entityId_));
        }
    }
}
