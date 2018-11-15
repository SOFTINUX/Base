// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    /// <summary>
    /// The posted data when modifying a role:
    /// - role new name
    /// - extensions to associate to role (if null no update)
    /// - permission level to associate to permissions (if update is performed)
    /// </summary>
    public class UpdateRoleAndGrantsViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        /// <summary>
        /// When null, no update, else replace existing associated extensions.
        /// </summary>
        /// <value></value>
        public List<string> Extensions { get; set; }
        /// <summary>
        /// When Extensions is not null, permission level to associate to extensions. Else ignored.
        /// </summary>
        /// <value></value>
        public string Permission { get; set; }
    }
}