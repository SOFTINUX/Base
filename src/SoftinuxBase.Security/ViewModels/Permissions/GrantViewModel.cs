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
        /// Data is sorted by extension name (outer key) and role name (inner key).
        /// Only the roles with linked permissions are present in this data set.
        /// </summary>
        public SortedDictionary<string, Dictionary<string, List<PermissionDisplay>>> PermissionsByRoleAndExtension { get; set; }

        /// <summary>
        /// Gets or sets for every extension, for every user, the attributed permissions.
        /// Data is sorted by extension (outer key) and user name (inner key).
        /// Only the users with linked permissions are present in this data set.
        /// </summary>
        public SortedDictionary<string, Dictionary<string, List<PermissionDisplay>>> PermissionsByUserAndExtension { get; set; }

        public GrantViewModel()
        {
            // TODO replace by new SortedDictionary<string, PermissionsDisplayDictionary>
            PermissionsByRoleAndExtension = new SortedDictionary<string, Dictionary<string, List<PermissionDisplay>>>();
            PermissionsByUserAndExtension = new SortedDictionary<string, Dictionary<string, List<PermissionDisplay>>>();
        }

        // TOTEST
        /// <summary>
        /// Return the names of all roles contained by this class.
        /// </summary>
        /// <returns>list of role names.</returns>
        public IEnumerable<string> RoleNames
        {
            get
            {
                HashSet<string> roleNames = new HashSet<string>();
                foreach (var permissionsByExtension in PermissionsByRoleAndExtension)
                {
                    foreach (var permissionByRole in permissionsByExtension.Value)
                        roleNames.Add(permissionByRole.Key);
                }

                return new List<string>(roleNames);
            }
        }

        // TOTEST
        /// <summary>
        /// Return the PermissionDisplay objects related to an extension, contained by this class.
        /// </summary>
        /// <param name="extensionName_">Name of an extension</param>
        /// <returns>list of <see cref="PermissionDisplay">permission descriptions.</see></returns>
        public IEnumerable<PermissionDisplay> GetPermissionDisplaysForRoles(string extensionName_)
        {
            var data = PermissionsByRoleAndExtension[extensionName_];
            HashSet<PermissionDisplay> permissionDisplaySet = new HashSet<PermissionDisplay>();
            foreach (var permissionDisplays in data.Values)
            {
                foreach (var permissionDisplay in permissionDisplays)
                {
                    permissionDisplaySet.Add(permissionDisplay);
                }
            }

            return new List<PermissionDisplay>(permissionDisplaySet);
        }
    }
}