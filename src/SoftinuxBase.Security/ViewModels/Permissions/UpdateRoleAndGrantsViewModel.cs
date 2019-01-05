// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    /// <summary>
    /// The posted data when modifying a role:
    /// - role new name
    /// - extensions/permission level to associate to role
    /// </summary>
    public class UpdateRoleAndGrantsViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        /// <summary>
        /// List of extension name with associated permission enum value.
        /// </summary>
        /// <value></value>
        public List<ExtensionPermissionValue> Grants { get; set; }
    }
}