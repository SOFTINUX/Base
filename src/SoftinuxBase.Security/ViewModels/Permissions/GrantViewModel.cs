// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Permission = SoftinuxBase.Security.Common.Enums.Permission;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    /// <summary>
    /// The view model is used to have a global view of permissions granting: for a role or a user, what kind of permission is granted, for every extension.
    /// </summary>
    public class GrantViewModel
    {
        /// <summary>
        /// For every extension, for every role (role name), the attributed permissions.
        /// Data is sorted by extension name and role name.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, Dictionary<string, List<Permission>>> PermissionsByRoleAndExtension {get; set;}

        /// <summary>
        /// For every extension, for every user, the attributed permissions.
        /// Data is sorted by extension name and user name.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, Dictionary<string, List<Permission>>> PermissionsByUserAndExtension {get; set;}

        public GrantViewModel()
        {
            PermissionsByRoleAndExtension = new SortedDictionary<string, Dictionary<string, List<Permission>>>();
            PermissionsByUserAndExtension = new SortedDictionary<string, Dictionary<string, List<Permission>>>();
        }
    }
}