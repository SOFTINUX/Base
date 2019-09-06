// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.EntityFramework")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SeedDatabase")]
// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SecurityTest")]
namespace SoftinuxBase.Security.Data.Abstractions
{
    public interface IUserPermissionRepository : IRepository
    {
        UserPermission FindBy(string userId_, string permissionId_);

        /// <summary>
        /// Find the links between an extension and any user, matching a permission level.
        /// </summary>
        /// <param name="extensionName_">Name of extension</param>
        /// <param name="level_">Level of <see cref="Common.Enums.Permission"/></param>
        /// <returns>List of <see cref="UserPermission"/>.</returns>
        IEnumerable<UserPermission> FindBy(string extensionName_, Common.Enums.Permission level_);

        IEnumerable<UserPermission> FilteredByUserId(string userId_);
        void Create(UserPermission entity_);
        void Edit(UserPermission entity_);
        void Delete(string userId_, string permissionId_);
        void DeleteAll();
    }
}
