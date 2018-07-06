// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace Security.Tools
{
    public static class UpdateRole
    {
        /// <summary>
        /// Check that a role with a close name exists.
        /// Compare role names ignoring case and spaces, so that
        /// "Role 1" and "role1" are considered homonyms (too close).
        /// </summary>
        /// <param name="roleName_"></param>
        /// <returns>true when a role with a close name is found</returns>
        public static bool CheckThatRoleWithCloseNameExists(string roleName_)
        {
            // TODO
            return false;
        }
    }
}