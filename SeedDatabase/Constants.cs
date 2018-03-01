// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace SeedDatabase
{
    public enum Role
    {
        AdministratorOwner,
        Administrator,
        User,
        Anonymous
    }

    public enum Group
    {
        AdministratorOwner,
        Administrators,
        Users,
        Anonymous
    }

    // public enum Permission
    // {
    //     Admin,
    //     Write,
    //     Read,
    //     Never
    // }

    public static class Constants
    {
        public static string GetRoleName(this Role role) // convenience method
        {
            return role.ToString();
        }
        public static string GetGroupName(this Group group) // convenience method
        {
            return group.ToString();
        }
        public static string GetPermissionName(this Infrastructure.Enums.Permission permission) // convenience method
        {
            return permission.ToString();
        }
    }
}