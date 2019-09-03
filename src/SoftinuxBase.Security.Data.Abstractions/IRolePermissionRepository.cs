// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.EntityFramework")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SeedDatabase")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("CommonTest")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SecurityTest")]
namespace SoftinuxBase.Security.Data.Abstractions
{
    internal interface IRolePermissionRepository : IRepository
    {
        IEnumerable<RolePermission> All();
        RolePermission FindBy(string roleId_, string extensionName_);
        IEnumerable<RolePermission> FindBy(string extensionName_, Common.Enums.Permission level_);
        IEnumerable<RolePermission> FilteredByRoleId(string roleId_);
        void Create(RolePermission entity_);
        void Edit(RolePermission entity_);
        void Delete(string roleId_, string extensionName_);
        void DeleteAll();
    }
}
