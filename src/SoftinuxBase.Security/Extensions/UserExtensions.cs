// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using System.Security.Claims;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Security.Permissions.Enums;

namespace SoftinuxBase.Security.Extensions
{
    /// <summary>
    /// Extension methods for currently logged user that is a ClaimsPrincipal.
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Check whether the currently logged user has some claim.
        /// Useful for Razor views.
        /// </summary>
        /// <remarks>This is a generic method</remarks>
        /// <param name="claimsPrincipal_">Application user object with all claims.</param>
        /// <param name="claimType_">Claim type.</param>
        /// <param name="claimValue_">Claim value.</param>
        /// <returns>True if a claim is found.</returns>
        public static bool HasClaim(this ClaimsPrincipal claimsPrincipal_, string claimType_, string claimValue_)
        {
            return claimsPrincipal_.Claims.FirstOrDefault(claim_ => claim_.Type == claimType_ && claim_.Value == claimValue_) != null;
        }

        /// <summary>
        /// Check whether the currently logged user has some claim of type Permission,
        /// defined by a permission level with a extension assembly name.
        /// Useful for Razor views.
        /// </summary>
        /// <param name="claimsPrincipal_">Application user object with all claims.</param>
        /// <param name="permission_">Permission level.</param>
        /// <param name="extensionAssemblySimpleName_">Assembly name of target extension.</param>
        /// <returns>True if a claim is found.</returns>
        public static bool HasPermissionClaim(this ClaimsPrincipal claimsPrincipal_, Permission permission_, string extensionAssemblySimpleName_)
        {
            return HasClaim(claimsPrincipal_, ClaimType.Permission, PermissionHelper.GetExtensionPermissionIdentifier(permission_, extensionAssemblySimpleName_));
        }
    }
}