// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Security.Claims;
using SoftinuxBase.Security.Permissions;

namespace SoftinuxBase.Security.FeatureAuthorize.Extensions
{
    public static class UserExtensions
    {
        // TOTEST
        
        /// <summary>
        /// Checks whether an user has a specific permission.
        /// </summary>
        /// <param name="user_">The Razor page user ("this.User").</param>
        /// <param name="permissionEnumType_">Permission enum type, for example <see cref="SoftinuxBase.Security.Permissions.Enums.Permissions"/></param>
        /// <param name="permission_">Permission enum value, for example a value in <see cref="SoftinuxBase.Security.Permissions.Enums.Permissions"/></param>
        /// <returns></returns>
        public static bool HasPermission(this ClaimsPrincipal user_, Type permissionEnumType_, short permission_)
        {
            var permissionsClaim = user_.Claims.SingleOrDefault(c => c.Type == PermissionConstants.PackedPermissionClaimType);
            // If user does not have the scope claim, get out of here
            if (permissionsClaim == null)
                return false;

            return (permissionsClaim.Value.ToPackedPermissions().ThisPermissionIsAllowed(StringExtensions.ToPolicyName(permissionEnumType_, permission_)));
        }
    }
}