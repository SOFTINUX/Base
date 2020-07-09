// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftinuxBase.Security.Permissions
{
    /// <summary>
    /// Permissions sorted by extension. This is used to represent role's or user's permissions.
    /// This is oriented to necessary data for permissions administration in UI/an API.
    /// <seealso cref="PermissionsDictionary"/>.
    /// </summary>
    public class PermissionsDisplayDictionary
    {
        /// <summary>
        /// Internal dictionary which key is the extension assembly short name and the values some PermissionDisplay classes (linked to role/user).
        /// </summary>
        internal readonly Dictionary<string, HashSet<PermissionDisplay>> Dictionary = new Dictionary<string, HashSet<PermissionDisplay>>();

        /// <summary>
        /// Add data to internal dictionary that contains the <see cref="PermissionDisplay"/> related to an extension.
        /// </summary>
        /// <param name="extensionName_">The extension name.</param>
        /// <param name="permissionEnumType_">The permission enum type for this extension.</param>
        public void Add(string extensionName_, Type permissionEnumType_)
        {
            Dictionary.TryGetValue(extensionName_, out var permissionDisplays);
            if (permissionDisplays != null)
            {
                return;
            }
            Dictionary[extensionName_] = PermissionDisplay.GetPermissionsToDisplay(extensionName_, permissionEnumType_).ToHashSet();
        }

        /// <summary>
        /// Check whether there is a permission item present.
        /// </summary>
        /// <returns>true when an item is present.</returns>
        public bool Any()
        {
            if (Dictionary.Keys.Count == 0)
            {
                return false;
            }

            return Dictionary[Dictionary.Keys.First()].Count != 0;
        }

        /// <summary>
        /// Get all the <see cref="PermissionDisplay"/> associated to an extension.
        /// </summary>
        /// <param name="extensionName_">Extension name.</param>
        /// <returns>List of <see cref="PermissionDisplay"/>.</returns>
        public HashSet<PermissionDisplay> Get(string extensionName_)
        {
            Dictionary.TryGetValue(extensionName_, out var permissionDisplays);
            return permissionDisplays;
        }

        /// <summary>
        /// Lookup a PermissionDisplay by extension name, group name, permission name.
        /// </summary>
        /// <param name="extensionName_">Extension name.</param>
        /// <param name="groupName_">Expected value of permission enum value "groupName" custom attribute.</param>
        /// <param name="permissionName_">Expected value of permission enum value "name" custom attribute.</param>
        /// <returns>PermissionDisplay or null.</returns>
        public PermissionDisplay Get(string extensionName_, string groupName_, string permissionName_)
        {
            Dictionary.TryGetValue(extensionName_, out var permissionDisplays);
            return permissionDisplays?.FirstOrDefault(permissionDisplay_ => permissionDisplay_.Section == groupName_ && permissionDisplay_.ShortName == permissionName_);
        }

        /// <summary>
        /// Lookup a PermissionDisplay by extension name and permission value.
        /// </summary>
        /// <param name="extensionName_">Extension name.</param>
        /// <param name="permissionValue_">Expected value of permission enum value "name" custom attribute.</param>
        /// <returns>PermissionDisplay or null.</returns>
        public PermissionDisplay Get(string extensionName_, short permissionValue_)
        {
            Dictionary.TryGetValue(extensionName_, out var permissionDisplays);
            return permissionDisplays?.FirstOrDefault(permissionDisplay_ => permissionDisplay_.PermissionEnumValue == permissionValue_);
        }
    }
}