// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between roles and permissions: permissions assigned to the role.
    /// </summary>
    public class RolePermission : IEntity
    {
        public string RoleId { get; set; }
        public string PermissionId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }

    }
}
