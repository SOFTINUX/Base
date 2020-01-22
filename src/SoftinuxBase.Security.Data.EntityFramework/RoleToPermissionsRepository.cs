// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class RoleToPermissionsRepository : RepositoryBase<RoleToPermissions>, IRoleToPermissionsRepository
    {
        /// <inheritdoc />
        public IEnumerable<RoleToPermissions> All()
        {
            return dbSet.ToList();
        }

        /// <inheritdoc/>
        public void DeleteAll()
        {
            dbSet.RemoveRange(dbSet.ToArray());
        }

        /// <inheritdoc/>
        public RoleToPermissions FindBy(string roleName_)
        {
            return dbSet.FirstOrDefault(role_ => role_.RoleName == roleName_);
        }

        /// <inheritdoc />
        public void Create(RoleToPermissions entity_)
        {
            dbSet.Add(entity_);
        }

        /// <inheritdoc/>
        public bool SetPermissions(string roleName_, PermissionsDictionary permissions_)
        {
            var roleToPermission = FindBy(roleName_);
            if (roleToPermission == null)
            {
                return false;
            }

            roleToPermission.Update(roleToPermission.Description, permissions_);
            return true;
        }
    }
}
