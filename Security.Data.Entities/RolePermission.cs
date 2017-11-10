// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between roles and permissions: permissions assigned to the role. Also stores the permission level.
    /// </summary>
    public class RolePermission : IEntity
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public int PermissionLevelId { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual PermissionLevel PermissionLevel { get; set; }

    }
}
