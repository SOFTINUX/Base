// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.PermissionPartsTests")]
namespace SoftinuxBase.Security.PermissionParts
{
    /// <summary>
    /// Permissions sorted by extension. This is used to represent user's permissions.
    /// </summary>
    public class PermissionsDictionary
    {
        internal readonly Dictionary<string, HashSet<short>> Dictionary = new Dictionary<string, HashSet<short>>();

        /// <summary>
        /// Add a permission if it doesn't already exist.
        /// </summary>
        /// <param name="extensionEnumType_">The enum type</param>
        /// <param name="permission_">Any value of <paramref name="extensionEnumType_"/></param>
        public void Add(Type extensionEnumType_, short permission_)
        {
            var extensionName = GetExtensionName(extensionEnumType_);
            Dictionary.TryGetValue(extensionName, out var permissions);
            if (permissions == null)
            {
                permissions = new HashSet<short>();
                Dictionary.Add(extensionName, permissions);
            }
            permissions.Add(permission_);
        }

        //TOTEST
        /// <summary>
        /// Add a group of permissions.
        /// </summary>
        /// <param name="extensionName_">Extension name</param>
        /// <param name="permissions_">Any value of enum defined in <paramref name="extensionName_"/> extension</param>
        internal void AddGrouped(string extensionName_, IEnumerable<short> permissions_)
        {
            Dictionary.TryGetValue(extensionName_, out var permissions);
            if (permissions == null)
            {
                permissions = new HashSet<short>();
                Dictionary.Add(extensionName_, permissions);
            }
            foreach(var permission in permissions_)
            {
                permissions.Add(permission);
            }
        }

        private string GetExtensionName(Type extensionEnumType_)
        {
            return extensionEnumType_.Assembly.GetName().Name;
        }

    }
}
