// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace Infrastructure.Enums
{
    public enum Permission
    {
        Admin,
        Write,
        Read,
        Never
    }

    public static class PermissionHelper
    {
        public static string GetPermissionName(this Permission permission_) // convenience method
        {
            return permission_.ToString();
        }
    }
    
}
