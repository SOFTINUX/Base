// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    public class UpdateRolePermissionViewModel
    {
        /// <summary>
        /// Name of role.
        /// </summary>
        [Required]
        public string RoleName { get; set; }

        /// <summary>
        /// Name of extension.
        /// </summary>
        [Required]
        public string ExtensionName { get; set; }
        
        /// <summary>
        /// Permission value (a value of the enum associated to the extension).
        /// </summary>
        public short PermissionValue { get; set; }

        /// <summary>
        /// When true, add a role to permission link, else remove.
        /// </summary>
        public bool Add { get; set; }
    }
}