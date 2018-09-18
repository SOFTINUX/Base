// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace SoftinuxBase.Security.Data.Entities
{
    /// <summary>
    /// Links between roles and permissions: permissions assigned to the role.
    /// </summary>
    public class RolePermission : IEntity
    {
        public RolePermission()
        {
            Scope = "Security";
        }

        public string RoleId { get; set; }
        public string PermissionId { get; set; }
        public string Scope { get; set; }

        public virtual IdentityRole<string> Role { get; set; }
        public virtual Permission Permission { get; set; }

    }
}
