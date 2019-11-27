// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class UserToRoleRepository : RepositoryBase<UserToRole>, IUserToRoleRepository
    {
        private readonly ILogger _logger;

        public UserToRoleRepository(): base() { }

        public UserToRoleRepository(ILoggerFactory loggerFactory_)
        {
            _logger = loggerFactory_.CreateLogger(GetType().FullName);
        }

        /// <inheritdoc />
        public IEnumerable<UserToRole> All()
        {
            return dbSet.ToList();
        }

        /// <inheritdoc />
        public void DeleteAll()
        {
            dbSet.RemoveRange(dbSet.ToArray());
        }

        /// <inheritdoc />
        public bool AddUserToRole(string userId_, string roleName_)
        {
            if (userId_ == null) throw new ArgumentNullException(nameof(userId_));
            if (roleName_ == null) throw new ArgumentNullException(nameof(roleName_));
            var userToRole = Find(userId_, roleName_);
            if (userToRole != null)
            {
                _logger.LogWarning($"The user already has the Role '{roleName_}'.");
                return false;
            }

            var roleToAdd = storageContext.Set<RoleToPermissions>().FirstOrDefault(roleToPermission_ => roleToPermission_.RoleName == roleName_);
            if (roleToAdd == null)
            {
                _logger.LogWarning($"User not added to Role, could not find the Role '{roleName_}'.");
                return false;
            }

            dbSet.Add(new UserToRole(userId_, roleToAdd));
            return true;
        }

        /// <inheritdoc />
        public UserToRole Find(string userId_, string roleName_)
        {
            return dbSet.FirstOrDefault(userToRole_ => userToRole_.UserId == userId_ && userToRole_.RoleName == roleName_);
        }
    }
}
