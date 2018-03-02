// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between users and permissions: permissions assigned to the user.
    /// </summary>
    public class UserPermission : IEntity
    {
        public  UserPermission()
        {
            Scope = "Security";
        }

        public string UserId { get; set; }
        public string PermissionId { get; set; }
        public string Scope { get; set; }

        public virtual User User { get; set; }
        public virtual Permission Permission { get; set; }

    }
}
