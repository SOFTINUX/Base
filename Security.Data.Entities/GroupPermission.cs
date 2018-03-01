// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between groups and permissions: permissions assigned to the group.
    /// </summary>
    public class GroupPermission : IEntity
    {
        public string GroupId { get; set; }
        public string PermissionId { get; set; }
        public string Scope { get; set; }

        public virtual Group Group { get; set; }
        public virtual Permission Permission { get; set; }

    }
}
