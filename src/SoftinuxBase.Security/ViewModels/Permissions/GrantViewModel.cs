// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using SoftinuxBase.Security.Permissions;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    /// <summary>
    /// The view model is used to have a global view of permissions granting: for a role or a user, what kind of permission is granted, for every extension.
    /// </summary>
    public class GrantViewModel
    {
        /// <summary>
        /// Gets or sets for every extension, for every role (role name), the attributed permissions.
        /// Data is sorted by extension name and role name.
        /// Only the roles with linked permissions are present in this data set.
        /// </summary>
        public SortedDictionary<string, Dictionary<string, List<PermissionDisplay>>> PermissionsByRoleAndExtension { get; set; }

        /// <summary>
        /// Gets or sets for every extension, for every user, the attributed permissions.
        /// Data is sorted by extension name and user name.
        /// Only the users with linked permissions are present in this data set.
        /// </summary>
        public SortedDictionary<string, Dictionary<string, List<PermissionDisplay>>> PermissionsByUserAndExtension { get; set; }

        public GrantViewModel()
        {
            // TODO replace by new SortedDictionary<string, PermissionsDisplayDictionary>
            PermissionsByRoleAndExtension = new SortedDictionary<string, Dictionary<string, List<PermissionDisplay>>>();
            PermissionsByUserAndExtension = new SortedDictionary<string, Dictionary<string, List<PermissionDisplay>>>();
        }
    }
}