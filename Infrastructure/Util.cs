// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Security.Claims;

namespace Infrastructure
{
    /// <summary>
    /// Utility for quick formatting.
    /// </summary>
    public static class Util
    {

        /// <summary>
        /// Formats the unique identifier that defines a permission and its scope.
        /// </summary>
        /// <param name="permission_"></param>
        /// <param name="scope_">The scope, equal to assembly name, given by Assembly.GetName().Name</param>
        /// <returns></returns>
        public static string GetScopedPermissionIdentifier(Enums.Permission permission_, string scope_)
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
            if(claim_.Type != Enums.ClaimType.Permission)
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
            return claimValue_.Substring(claimValue_.LastIndexOf(".")+1);
        }

        /// <summary>
        /// From the value of a claim which type is Permission, gets the first part of the string that represents the permission scope.
        /// </summary>
        /// <param name="claim_"></param>
        /// <returns></returns>
        public static string GetPermissionScope(Claim claim_)
        {
            if(claim_.Type != Enums.ClaimType.Permission)
                return null;
            return GetPermissionScope(claim_.Value);
        }

        /// <summary>
        /// From the value of a claim which type is Permission, gets the first part of the string that represents the permission scope.
        /// </summary>
        /// <param name="claim_"></param>
        /// <returns></returns>
        public static string GetPermissionScope(string claimValue_)
        {
            return claimValue_.Substring(0, claimValue_.Length - claimValue_.LastIndexOf("."));
        }
    }
}
