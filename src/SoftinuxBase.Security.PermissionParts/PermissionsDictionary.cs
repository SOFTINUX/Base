// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

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
        /// <param name="permission_"></param>
        public void Add(Permissions permission_)
        {
            var extensionName = GetExtensionName(permission_);
            Dictionary.TryGetValue(extensionName, out var permissions);
            if(permissions == null)
            {
                permissions = new HashSet<short>();
                Dictionary.Add(extensionName, permissions);
            }
            permissions.Add((short)permission_);

        }

        private string GetExtensionName(Permissions permission_)
        {
            return permission_.GetType().Assembly.GetName().Name;
        }

    }
}
