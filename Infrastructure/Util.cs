// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

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
    }
}
