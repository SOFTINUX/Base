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
    //public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    //{

    //    public UserRole FindBy(string userId_, string roleId_)
    //    {
    //        return dbSet.FirstOrDefault(e_ => e_.UserId == userId_ && e_.RoleId == roleId_);
    //    }

    //    public IEnumerable<UserRole> FilteredByRoleId(string roleId_)
    //    {
    //        return dbSet.Where(e_ => e_.RoleId == roleId_).ToList();
    //    }

    //    public IEnumerable<UserRole> FilteredByUserId(string userId_)
    //    {
    //        return dbSet.Where(e_ => e_.UserId == userId_).ToList();
    //    }

    //    public virtual void Create(UserRole entity_)
    //    {
    //        dbSet.Add(entity_);
    //    }

    //    public virtual void Edit(UserRole entity_)
    //    {
    //        storageContext.Entry(entity_).State = EntityState.Modified;
    //    }

    //    public void Delete(string userId_, string roleId_)
    //    {
    //        dbSet.Remove(FindBy(userId_, roleId_));
    //    }

    //}
}
