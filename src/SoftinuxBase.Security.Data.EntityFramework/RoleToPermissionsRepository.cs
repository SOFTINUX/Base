// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtCore.Data.EntityFramework;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses;
using SoftinuxBase.Security.PermissionParts;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class RoleToPermissionsRepository : RepositoryBase<RoleToPermissions>, IRoleToPermissionsRepository
    {
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

        public void Create(RoleToPermissions entity_)
        {
            dbSet.Add(entity_);
        }

        /// <inheritdoc/>
        public bool SetPermissions(string roleName_, ICollection<Permissions> permissions_)
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
