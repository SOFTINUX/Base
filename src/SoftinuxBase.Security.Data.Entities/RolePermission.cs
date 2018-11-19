// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Data.Entities.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace SoftinuxBase.Security.Data.Entities
{
    /// <summary>
    /// Links between roles and permissions: permissions assigned to the role.
    /// Default value for "Scope": "SoftinuxBase.Security".
    /// </summary>
    public class RolePermission : IEntity
    {
        private string _name;
        public RolePermission()
        {
            Extension = "SoftinuxBase.Security";
            PermissionId = Guid.NewGuid().ToString();
        }

        public string RoleId { get; set; }
        public string Name
        {
            get {
                return _name;
            }
            set
            {
                _name = value;
                NormalizedName = value.ToLowerInvariant();
            }
        }
        public string Extension { get; set; }
        public string PermissionId { get; private set; }
        public string NormalizedName { get; private set; }

        public virtual IdentityRole<string> Role { get; set; }
        public virtual Permission Permission { get; set; }

    }
}
