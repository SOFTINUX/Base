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
        /// Gets or sets for every extension, for every permission, the roles (role names).
        /// Data is sorted by extension name (outer key) and permission display object (inner key).
        /// Only the permission display objects and roles linked together are present in this data set.
        /// </summary>
        public SortedDictionary<string, Dictionary<PermissionDisplay, List<string>>> RolesWithPermissions { get; set; }

        /// <summary>
        /// Gets or sets for every extension, for every permission, the users (user names).
        /// Data is sorted by extension name (outer key) and permission display object (inner key).
        /// Only the permission display objects and users linked together are present in this data set.
        /// </summary>
        public SortedDictionary<string, Dictionary<PermissionDisplay, List<string>>> UsersWithPermissions { get; set; }

        public GrantViewModel()
        {
            RolesWithPermissions = new SortedDictionary<string, Dictionary<PermissionDisplay, List<string>>>();
            UsersWithPermissions = new SortedDictionary<string, Dictionary<PermissionDisplay, List<string>>>();
        }
    }
}