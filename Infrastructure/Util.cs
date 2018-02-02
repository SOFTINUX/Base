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
        /// Formats the unique identifier of a permission that has a code (name) and an origin extension.
        /// </summary>
        /// <param name="permissionCode_"></param>
        /// <param name="originExtensionAssemblyName_">The origin extension assembly name, given by Assembly.GetName().Name</param>
        /// <returns></returns>
        public static string GetPermissionUniqueIdentifier(string permissionCode_, string originExtensionAssemblyName_)
        {
            return $"{originExtensionAssemblyName_}.{permissionCode_}";
        }
    }
}
