// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    public class UpdateRolePermissionViewModel
    {
        /// <summary>
        /// Name of role.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Assembly-qualified permission type full name.
        /// </summary>
        public string PermissionType { get; set; }
        
        /// <summary>
        /// Permission value.
        /// </summary>
        public short PermissionValue { get; set; }

        /// <summary>
        /// When true, add a role to permission link, else remove.
        /// </summary>
        public bool Add { get; set; }
    }
}