// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    /// <summary>
    /// The posted data when modifying a role.
    /// </summary>
    public class UpdateRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}