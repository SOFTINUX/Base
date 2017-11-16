// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between users and permissions: permissions assigned to the user. Also stores the permission level.
    /// </summary>
    public class UserPermission : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PermissionId { get; set; }

        public int PermissionLevelId { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual User User { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual PermissionLevel PermissionLevel { get; set; }

    }
}
