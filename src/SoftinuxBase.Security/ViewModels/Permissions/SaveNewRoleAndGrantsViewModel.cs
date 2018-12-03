// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    /// <summary>
    /// The posted data when adding a new role:
    /// - role name
    /// - extensions to associate to role
    /// - permission level.
    /// </summary>
    public class SaveNewRoleAndGrantsViewModel
    {
        public string RoleName {get; set;}

        public List<string> Extensions {get; set;}

        public string PermissionValue {get; set;}
    }
}