// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;

namespace Infrastructure.Enums
{
    public enum Permission
    {
        Never = 0,
        Read = 1,
        Write = 2,
        Admin = 4
    }

    public static class PermissionHelper
    {
        public static Permission FromId(string id_)
        {
            Enum.TryParse(id_, out Permission perm);
            return perm;
        }

        public static string GetPermissionName(this Permission permission_) // convenience method
        {
            return permission_.ToString();
        }
    }

}
