// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between groups and users.
    /// </summary>
    public class GroupUser : IEntity
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
    }
}
