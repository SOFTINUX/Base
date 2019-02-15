// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using System.Security.Claims;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Common.Enums;

namespace SoftinuxBase.Security.Extensions
{
    /// <summary>
    /// Extension methods for currently logged user that is a ClaimsPrincipal.
    /// </summary>
    public static class UserExtensions
    {
            /// <summary>
            /// Generic method to check whether the currently logged user has some claim.
            /// Useful for Razor views.
            /// </summary>
            /// <param name="claimsPrincipal_"></param>
            /// <param name="claimType_"></param>
            /// <param name="claimValue_"></param>
            /// <returns></returns>
            public static bool HasClaim(this ClaimsPrincipal claimsPrincipal_, string claimType_, string claimValue_)
            {
                return claimsPrincipal_.Claims.FirstOrDefault(c_ => c_.Type == claimType_ && c_.Value == claimValue_) != null;
            }

            /// <summary>
            /// Method to check whether the currently logged user hasc some claim of type Permission,
            /// defined by a permission (enum value) and a scope (assembly simple name).
            /// Useful for Razor views.
            /// </summary>
            /// <param name="claimsPrincipal_"></param>
            /// <param name="permission_"></param>
            /// <param name="extensionAssemblySimpleName_"></param>
            /// <returns></returns>
            public static bool HasPermissionClaim(this ClaimsPrincipal claimsPrincipal_, Permission permission_, string extensionAssemblySimpleName_)
            {
                return HasClaim(claimsPrincipal_, ClaimType.Permission, PermissionHelper.GetExtensionPermissionIdentifier(permission_, extensionAssemblySimpleName_));
            }
    }
}