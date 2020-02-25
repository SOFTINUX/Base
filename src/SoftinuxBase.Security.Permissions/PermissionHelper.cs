// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using SoftinuxBase.Security.Permissions.Enums;

namespace SoftinuxBase.Security.Permissions
{
    /*
        The main permissions helper class.
        Contains all methods for permissions.
    */
    /// <summary>
    /// The main permissions helper class.
    ///
    /// Contains all methods for permissions.
    /// </summary>
    public static class PermissionHelper
    {
        [Obsolete]
        public static Permission FromId(string id_)
        {
            Enum.TryParse(id_, out Permission perm);
            return perm;
        }

        /// <summary>
        /// TODO DOCUMENT ME
        /// </summary>
        /// <param name="name_">Permission name</param>
        /// <returns>Permissions name from enum</returns>
        public static Permission FromName(string name_)
        {
            Enum.TryParse(name_, out Permission perm);
            return perm;
        }

        /// <summary>
        /// Extension method to get the string representation of the enum value.
        /// </summary>
        /// <param name="permissionLevel_">Permission level.</param>
        /// <returns>String represent permission level.</returns>
        public static string GetPermissionName(this Permission permissionLevel_)
        {
            return permissionLevel_.ToString();
        }

        /// <summary>
        /// Formats the unique identifier that associates a permission level and an extension name.
        /// </summary>
        /// <param name="permissionLevel_">Permission level.</param>
        /// <param name="extensionName_">The extension name, equal to assembly name, given by <c>Assembly.GetName().Name</c>.</param>
        /// <returns>String reprensent extension name with permission level.</returns>
        public static string GetExtensionPermissionIdentifier(Permission permissionLevel_, string extensionName_)
        {
            return $"{extensionName_}.{permissionLevel_}";
        }

        /// <summary>
        /// From the value of a claim which type is Permission, gets the second part of the string that represents the permission level (Admin, Write...).
        /// </summary>
        /// <param name="claim_">Claim of type permission.</param>
        /// <returns>The second part of the string that represents the permission level.</returns>
        public static string GetPermissionLevel(Claim claim_)
        {
            if (claim_.Type != ClaimType.Permission)
            {
                return null;
            }

            return GetPermissionLevel(claim_.Value);
        }

        /// <summary>
        /// From the value of a claim which type is Permission, gets the second part of the string that represents the permission level (Admin, Write...).
        /// </summary>
        /// <param name="claimValue_">Value of a claim of type permission.</param>
        /// <returns>The second part of the string that represents the permission level.</returns>
        public static string GetPermissionLevel(string claimValue_)
        {
            return claimValue_.Substring(claimValue_.LastIndexOf(".", StringComparison.Ordinal) + 1);
        }

        /// <summary>
        /// From a claim which type is Permission, gets the first part of the string that represents the name of extension concerned by this permission level.
        /// </summary>
        /// <param name="claim_">Claim of type permission.</param>
        /// <returns>The first part of the string that represents the name of extension concerned by this permission level.</returns>
        public static string GetExtensionName(Claim claim_)
        {
            return claim_.Type != ClaimType.Permission ? null : GetExtensionName(claim_.Value);
        }

        /// <summary>
        /// From the value of a claim which type is Permission, gets the first part of the string that represents the name of extension concerned by this permission level.
        /// </summary>
        /// <param name="claimValue_">Value of a claim of type permission.</param>
        /// <returns>The first part of the string that represents the name of extension concerned by this permission level.</returns>
        public static string GetExtensionName(string claimValue_)
        {
            return claimValue_.Substring(0, claimValue_.Length - claimValue_.LastIndexOf(".", StringComparison.Ordinal));
        }

        /// <summary>
        /// Get the list of values from Permission enum that are lower or equal to a value (if Write, then Read and Write).
        /// </summary>
        /// <param name="permission_">Permissions level.</param>
        /// <returns>List of permission levels.</returns>
        public static List<Permission> GetLowerOrEqual(Permission permission_)
        {
            return new List<Permission>((Permission[])Enum.GetValues(typeof(Permission))).Where(p_ => (int)p_ <= (int)permission_).ToList();
        }
    }
}
