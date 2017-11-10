// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Infrastructure
{
    public static class PolicyUtil
    {
        /// <summary>
        /// The access level, R for read-only, RW for read-write.
        /// </summary>
        public enum AccessLevel
        {
            R,
            // ReSharper disable once InconsistentNaming
            RW
        }
        /// <summary>
        /// Suffix used to build the claim value from the permission unique identifier.
        /// </summary>
        public const string READ_WRITE_SUFFIX = "|RW";

        /// <summary>
        /// Suffix used to build the claim value from the permission unique identifier.
        /// </summary>
        public const string READ_ONLY_SUFFIX = "|R";

        /// <summary>
        /// Gets the claim value from the permission unique ID and the access level
        /// </summary>
        /// <param name="permissionUniqueId_"></param>
        /// <param name="write_">true when write access level, false when read</param>
        /// <returns></returns>
        public static string GetClaimValue(string permissionUniqueId_, bool write_)
        {
            return permissionUniqueId_ + (write_
                       ? PolicyUtil.READ_WRITE_SUFFIX
                       : PolicyUtil.READ_ONLY_SUFFIX);
        }

        /// <summary>
        /// Formats the policy name associated with a permission that has a code and an origin extension.
        /// </summary>
        /// <param name="permissionCode_"></param>
        /// <param name="write_"></param>
        /// <typeparam name="T">Extension metadata</typeparam>
        /// <returns></returns>
        public static string GetPolicyName<T>(string permissionCode_, bool write_) where T : IExtensionMetadata
        {
            return $"{permissionCode_}|{typeof(T).Assembly.GetName().Name}|{(write_ ? READ_WRITE_SUFFIX : READ_ONLY_SUFFIX)}";
        }

        /// <summary>
        /// Formats the policy name associated with a permission that has a code and an origin extension, and an access level.
        /// </summary>
        /// <param name="permissionCode_"></param>
        /// <param name="originExtensionAssemblyName_">The origin extension assembly name, given by Assembly.GetName().Name</param>
        /// <param name="accessLevel_"></param>
        /// <returns></returns>
        public static string GetPolicyName(string permissionCode_, string originExtensionAssemblyName_, AccessLevel accessLevel_)
        {
            return $"{GetPermissionUniqueIdentifier(permissionCode_, originExtensionAssemblyName_)}|{(accessLevel_)}";
        }

        /// <summary>
        /// Formats the unique identifier of a permission that has a code and an origin extension.
        /// </summary>
        /// <param name="permissionCode_"></param>
        /// <param name="originExtensionAssemblyName_">The origin extension assembly name, given by Assembly.GetName().Name</param>
        /// <returns></returns>
        public static string GetPermissionUniqueIdentifier(string permissionCode_, string originExtensionAssemblyName_)
        {
            return
                $"{permissionCode_}|{originExtensionAssemblyName_}";
        }
    }
}
