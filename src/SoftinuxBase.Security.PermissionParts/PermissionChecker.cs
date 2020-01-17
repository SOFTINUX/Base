// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX.
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SoftinuxBase.Security.PermissionParts
{
    public static class PermissionChecker
    {
        /// <summary>
        /// This is used by the policy provider to check the permission name string
        /// </summary>
        /// <param name="packedPermissions"></param>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        public static bool ThisPermissionIsAllowed(this string packedPermissions, string permissionName)
        {
            var usersPermissions = packedPermissions.UnpackPermissionsFromString().ToArray();

            if (!Enum.TryParse(permissionName, true, out Permissions permissionToCheck))
                throw new InvalidEnumArgumentException($"{permissionName} could not be converted to a {nameof(Permissions)}.");

            return usersPermissions.UserHasThisPermission(permissionToCheck);
        }

        /// <summary>
        /// This is the main checker of whether a user permissions allows them to access something with the given permission
        /// </summary>
        /// <param name="usersPermissions"></param>
        /// <param name="permissionToCheck"></param>
        /// <returns></returns>
        /// TO BE REMOVED
        [Obsolete]
        public static bool UserHasThisPermission(this Permissions[] usersPermissions, Permissions permissionToCheck)
        {
            return usersPermissions.Contains(permissionToCheck) || usersPermissions.Contains(Permissions.AccessAll);
        }

        /// <summary>
        /// This is used by the policy provider to check the permission name string.
        /// </summary>
        /// <param name="packedPermissions_"></param>
        /// <param name="permissionName_">String that identifies the permission (policy).</param>
        /// <returns></returns>
        public static bool ThisPermissionIsAllowed(this Dictionary<string, string> packedPermissions_, string permissionName_)
        {
            var usersPermissions = packedPermissions_.UnpackPermissions();
            var nameParts = permissionName_.ParsePolicyName();
            return UserHasThisPermission(usersPermissions, nameParts.Item1, nameParts.Item2);
        }

        /// <summary>
        /// This is the main checker of whether a user permissions allows them to access something with the given permission.
        /// </summary>
        /// <param name="usersPermissions_">The (unpacked) permissions of the user.</param>
        /// <param name="permissionToCheckEnumTypeFullName_">The fullname of the type of the permissions enum.</param>
        /// <param name="permissionToCheck_">The permission value.</param>
        /// <returns></returns>
        public static bool UserHasThisPermission(this PermissionsDictionary usersPermissions_, string permissionToCheckEnumTypeFullName_, short permissionToCheck_)
        {
            return usersPermissions_.Contains(permissionToCheckEnumTypeFullName_, permissionToCheck_);
        }
    }
}