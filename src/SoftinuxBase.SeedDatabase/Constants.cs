// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.SeedDatabase
{
    public enum Role
    {
        /// <summary>
        /// Administrator role.
        /// </summary>
        Administrator,

        /// <summary>
        /// User role.
        /// </summary>
        User,

        /// <summary>
        /// Anonymous role.
        /// </summary>
        Anonymous
    }

    public static class Constants
    {
        public static string GetRoleName(this Role role) // convenience method
        {
            return role.ToString();
        }
    }
}