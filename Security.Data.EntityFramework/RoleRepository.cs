// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        /// <summary>
        /// Finds a role by id (primary key).
        /// </summary>
        /// <param name="entityId_"></param>
        /// <returns></returns>
        public virtual Role FindById(string entityId_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
        }

        public virtual Role FindBy(string code_, string originExtensionAssemblyName_)
        {
            return dbSet.FirstOrDefault(e_ => e_.Name == code_);
        }

        public virtual IEnumerable<Role> All()
        {
            return dbSet.ToList();
        }

        public virtual void Create(Role entity_)
        {
            dbSet.Add(entity_);
        }

        public virtual void Edit(Role entity_)
        {
            storageContext.Entry(entity_).State = EntityState.Modified;
        }

        public virtual void Delete(string entityId_)
        {
            var entity = dbSet.FirstOrDefault(e_ => e_.Id == entityId_);
            if(entity != null)
                dbSet.Remove(entity);
        }

    }
}
