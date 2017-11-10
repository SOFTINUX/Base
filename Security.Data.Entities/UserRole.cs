// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between users and roles.
    /// </summary>
    public class UserRole : IEntity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
