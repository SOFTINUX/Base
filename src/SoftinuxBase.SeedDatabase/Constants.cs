// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Common.Enums;

namespace SoftinuxBase.SeedDatabase
{
    public enum Role
    {
        Administrator,
        User,
        Anonymous
    }

    public static class Constants
    {
        public static string GetRoleName(this Role role) // convenience method
        {
            return role.ToString();
        }

        public static string GetPermissionName(this Permission permission) // convenience method
        {
            return PermissionHelper.GetPermissionName(permission);
        }
    }
}