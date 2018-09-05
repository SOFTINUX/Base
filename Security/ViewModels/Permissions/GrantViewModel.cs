// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Security.Common.Enums;

namespace Security.ViewModels.Permissions
{
    /// <summary>
    /// The view model is used to have a global view of permissions granting: for a role or a user, what kind of permission is granted, for every extension.
    /// </summary>
    public class GrantViewModel
    {
        /// <summary>
        /// For every extension, for every role (role name), the attributed permissions.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, List<Permission>>> PermissionsByRoleAndScope {get; set;}

        /// <summary>
        /// For every scope, for every user, the attributed permissions.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, List<Permission>>> PermissionsByUserAndScope {get; set;}

        public GrantViewModel()
        {
            PermissionsByRoleAndScope = new Dictionary<string, Dictionary<string, List<Permission>>>();
            PermissionsByUserAndScope = new Dictionary<string, Dictionary<string, List<Permission>>>();
        }
    }
}