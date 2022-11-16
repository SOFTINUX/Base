// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Gets or sets all names of the roles that have associated permissions (roles found in RoleToPermissions table).
        /// </summary>
        public List<string> RoleNames { get; set; }

        public GrantViewModel()
        {
            RolesWithPermissions = new SortedDictionary<string, Dictionary<PermissionDisplay, List<string>>>();
            UsersWithPermissions = new SortedDictionary<string, Dictionary<PermissionDisplay, List<string>>>();
            RoleNames = new List<string>();
        }

        /// <summary>
        /// Gets all the <see cref="string"/> permission sections associated to an extension, for roles.
        /// </summary>
        /// <param name="extensionName_">Extension name.</param>
        /// <returns>List of <see cref="string"/> sections.</returns>
        public HashSet<string> GetPermissionSectionsForRoles(string extensionName_)
        {
            RolesWithPermissions.TryGetValue(extensionName_, out var permissionDisplaysWithRoles);
            if (permissionDisplaysWithRoles == null)
            {
                return new HashSet<string>();
            }

            return permissionDisplaysWithRoles.Keys.Select(permissionDisplay_ => permissionDisplay_.Section).ToHashSet();
        }

        /// <summary>
        /// Gets all the <see cref="PermissionDisplay"/> associated to an extension and a permission section, for roles.
        /// </summary>
        /// <param name="extensionName_">Extension name.</param>
        /// <param name="section_">Permission section.</param>
        /// <returns>List of <see cref="PermissionDisplay"/>.</returns>
        public IEnumerable<KeyValuePair<PermissionDisplay, List<string>>> GetPermissionDisplaysWithRoles(string extensionName_, string section_)
        {
            RolesWithPermissions.TryGetValue(extensionName_, out var permissionDisplaysWithRoles);
            if (permissionDisplaysWithRoles == null)
            {
                yield break;
            }

            foreach (var permissionDisplaysWithRole in permissionDisplaysWithRoles)
            {
                if (permissionDisplaysWithRole.Key.ExtensionName == extensionName_ && permissionDisplaysWithRole.Key.Section == section_)
                {
                    yield return permissionDisplaysWithRole;
                }
            }
        }
    }
}