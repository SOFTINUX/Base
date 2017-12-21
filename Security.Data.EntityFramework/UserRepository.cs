// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

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
        public virtual User FindById(string entityId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
        }

        public User FindBy(string firstName_, string lastName_, string displayName_, string originExtensionAssemblyName_)
        {
            return dbSet.FirstOrDefault(e_ => e_.FirstName == firstName_
            && e_.LastName == lastName_);
        }

        /// <summary>
        /// OBSOLETE
        /// Get an user by credential identifier. Useful for unit tests.
        /// </summary>
        /// <param name="identifier_"></param>
        /// <returns></returns>
        public virtual User WithCredentialIdentifier(string identifier_)
        {
            return null;
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
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public virtual void Delete(string entityId_)
        {
            dbSet.Remove(FindById(entityId_));
        }
    }
}
