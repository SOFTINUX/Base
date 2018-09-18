// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using SoftinuxBase.Security.Common.Enums;

namespace SoftinuxBase.Security.Common
{
    public static class PermissionHelper
    {
        public static Permission FromId(string id_)
        {
            Enum.TryParse(id_, out Permission perm);
            return perm;
        }

        /// <summary>
        /// Extension method to get the string representation of the enum value.
        /// </summary>
        /// <param name="permission_"></param>
        /// <returns></returns>
        public static string GetPermissionName(this Permission permission_)
        {
            return permission_.ToString();
        }

        /// <summary>
        /// Formats the unique identifier that defines a permission and its scope.
        /// </summary>
        /// <param name="permission_"></param>
        /// <param name="scope_">The scope, equal to assembly name, given by Assembly.GetName().Name</param>
        /// <returns></returns>
        public static string GetScopedPermissionIdentifier(Permission permission_, string scope_)
        {
            return $"{scope_}.{permission_}";
        }

        /// <summary>
        /// From the value of a claim which type is Permission, gets the second part of the string that represents the permission level (Admin, Write...).
        /// </summary>
        /// <param name="claim_"></param>
        /// <returns></returns>
        public static string GetPermissionLevel(Claim claim_)
        {
            if (claim_.Type != ClaimType.Permission)
                return null;
            return GetPermissionLevel(claim_.Value);
        }

        /// <summary>
        /// From the value of a claim which type is Permission, gets the second part of the string that represents the permission level (Admin, Write...).
        /// </summary>
        /// <param name="claim_"></param>
        /// <returns></returns>
        public static string GetPermissionLevel(string claimValue_)
        {
            return claimValue_.Substring(claimValue_.LastIndexOf(".", StringComparison.Ordinal) + 1);
        }

        /// <summary>
        /// From the value of a claim which type is Permission, gets the first part of the string that represents the permission scope.
        /// </summary>
        /// <param name="claim_"></param>
        /// <returns></returns>
        public static string GetPermissionScope(Claim claim_)
        {
            if (claim_.Type != ClaimType.Permission)
                return null;
            return GetPermissionScope(claim_.Value);
        }

        /// <summary>
        /// From the value of a claim which type is Permission, gets the first part of the string that represents the permission scope.
        /// </summary>
        /// <param name="claimValue_"></param>
        /// <returns></returns>
        public static string GetPermissionScope(string claimValue_)
        {
            return claimValue_.Substring(0, claimValue_.Length - claimValue_.LastIndexOf(".", StringComparison.Ordinal));
        }

        /// <summary>
        /// Get the list of values from Permission enum that are lower or equal to a value (if Write, then Read and Write).
        /// </summary>
        /// <param name="permission_"></param>
        /// <returns></returns>
        public static List<Permission> GetLowerOrEqual(Permission permission_)
        {
            return new List<Permission>((Permission[])Enum.GetValues(typeof(Permission))).Where(p_ => (int) p_ <= (int) permission_).ToList();
        }
    }
}
