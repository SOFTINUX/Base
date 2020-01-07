// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.EntityFramework")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SeedDatabase")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Tests.Common")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SecurityTests")]
namespace SoftinuxBase.Security.Data.Abstractions
{
    public interface IRolePermissionRepository : IRepository
    {
        /// <summary>
        /// Return all role permission records.
        /// </summary>
        /// <returns><see cref="IEnumerable{RolePermission}" /> of <see cref="RolePermission" />.</returns>
        IEnumerable<RolePermission> All();

        /// <summary>
        /// Return all role permission records joining permissions from permission table.
        /// </summary>
        /// <returns>Return a <see cref="IEnumerable{RolePermission}" /> of <see cref="RolePermission" />.</returns>
        IEnumerable<RolePermission> AllRolesWithPermissions();

        /// <summary>
        /// Find the link between an extension and a role.
        /// </summary>
        /// <param name="roleId_">ID of role</param>
        /// <param name="extensionName_">Name of extension</param>
        /// <returns><see cref="RolePermission"/>.</returns>
        RolePermission FindBy(string roleId_, string extensionName_);

        /// <summary>
        /// Find the links between an extension and any role, matching a permission level.
        /// </summary>
        /// <param name="extensionName_">Name of extension</param>
        /// <param name="level_">Level of <see cref="Common.Enums.Permission"/></param>
        /// <returns>List of <see cref="RolePermission"/> with role name also loaded in associated Role</returns>
        IEnumerable<RolePermission> FindBy(string extensionName_, Common.Enums.Permission level_);

        /// <summary>
        ///  Return filtered roles list.
        /// </summary>
        /// <param name="roleId_">role id.</param>
        /// <returns>Return a <see cref="IEnumerable{RolePermission}" /> of <see cref="RolePermission" />.</returns>
        IEnumerable<RolePermission> FilteredByRoleId(string roleId_);

        /// <summary>
        /// Create a Role Permission record
        /// </summary>
        /// <param name="entity_"><see cref="RolePermission" />.</param>
        void Create(RolePermission entity_);

        /// <summary>
        /// Modify a Role Permission field
        /// </summary>
        /// <param name="entity_"><see cref="RolePermission" />.</param>
        void Edit(RolePermission entity_);

        /// <summary>
        /// Delete.
        /// </summary>
        /// <param name="roleId_"></param>
        /// <param name="extensionName_"></param>
        void Delete(string roleId_, string extensionName_);

        /// <summary>
        /// Delete All.
        /// </summary>
        void DeleteAll();
    }
}
