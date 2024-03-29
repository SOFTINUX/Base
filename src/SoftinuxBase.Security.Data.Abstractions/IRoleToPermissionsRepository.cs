﻿// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

using ExtCore.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Permissions;

namespace SoftinuxBase.Security.Data.Abstractions
{
    public interface IRoleToPermissionsRepository : IRepository
    {
        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <returns>All <see cref="RoleToPermissions"/> records.</returns>
        IEnumerable<RoleToPermissions> All();

        /// <summary>
        /// Delete all roles to permissions records.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Delete a record.
        /// </summary>
        /// <param name="entity_"></param>
        void Delete(RoleToPermissions entity_);

        /// <summary>
        /// Find a record by role name.
        /// </summary>
        /// <param name="roleName_">Name of role.</param>
        /// <returns><see cref="RoleToPermissions"/> if any.</returns>
        RoleToPermissions FindBy(string roleName_);

        /// <summary>
        /// Create a role to permissions record.
        /// </summary>
        /// <param name="entity_">role to permissions data.</param>
        void Create(RoleToPermissions entity_);

        /// <summary>
        /// Set new permissions to a role, erasing old values. The record for the role must already exist.
        /// </summary>
        /// <param name="roleName_">Name of role.</param>
        /// <param name="permissions_">New permissions.</param>
        /// <returns>true when an existing link was updated.</returns>
        bool SetPermissions(string roleName_, PermissionsDictionary permissions_);
    }
}
