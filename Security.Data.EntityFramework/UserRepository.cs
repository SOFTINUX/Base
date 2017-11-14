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
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public virtual User WithKey(int entityId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
        }
        /// <summary>
        /// Get an user by credential identifier. Useful for unit tests.
        /// </summary>
        /// <param name="identifier_"></param>
        /// <returns></returns>
        public virtual User WithCredentialIdentifier(string identifier_)
        {
            Credential c = ((DbContext)storageContext).Set<Credential>().FirstOrDefault(c_ => c_.Identifier == identifier_);
            return c == null ? null : WithKey(c.UserId);
        }


        public virtual IEnumerable<User> All()
        {
            return dbSet.ToList();
        }

        public virtual void Create(User entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(User entity_)
        {
            ((DbContext)storageContext).Entry(entity_).State = EntityState.Modified;
        }

        public virtual void Delete(int entityId_)
        {
            dbSet.Remove(WithKey(entityId_));
        }
    }
}
