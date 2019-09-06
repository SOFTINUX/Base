// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.EntityFramework")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SeedDatabase")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("CommonTest")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SecurityTest")]
namespace SoftinuxBase.Security.Data.Abstractions
{
    public interface IRolePermissionRepository : IRepository
    {
        IEnumerable<RolePermission> All();

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

        IEnumerable<RolePermission> FilteredByRoleId(string roleId_);
        void Create(RolePermission entity_);
        void Edit(RolePermission entity_);
        void Delete(string roleId_, string extensionName_);
        void DeleteAll();
    }
}
