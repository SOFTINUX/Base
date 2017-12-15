// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using Infrastructure.Interfaces;

namespace Security.Common
{
    /// <summary>
    /// Utility to quickly format a policy name, a claim value, when you know which application permission matches this claim or policy.
    /// TODO better renaming of these functions.
    /// TODO REMOVE ToLowerInvariant everywhere in code.
    /// </summary>
    public static class PolicyUtil
    {
        /// <summary>
        /// Gets the claim value: permission unique ID, lowercased.
        /// </summary>
        /// <param name="permissionUniqueId_"></param>
        /// <returns></returns>
        public static string GetClaimValue(string permissionUniqueId_)
        {
            return permissionUniqueId_.ToLowerInvariant();
        }

        /// <summary>
        /// Formats the policy name (= also claim value), knowing a permission code (name) and its "T" origin extension.
        /// Note: the returned value is lower case.
        /// </summary>
        /// <param name="permissionCode_"></param>
        /// <typeparam name="T">Extension metadata</typeparam>
        /// <returns></returns>
        public static string GetPolicyName<T>(string permissionCode_) where T : IExtensionMetadata
        {
            return $"{typeof(T).Assembly.GetName().Name}.{permissionCode_}".ToLowerInvariant();
        }

        /// <summary>
        /// Formats the policy name (= also claim value), knowing a permission code (name) and its origin extension.
        /// Note: the returned value is lower case.
        /// </summary>
        /// <param name="permissionCode_"></param>
        /// <param name="originExtensionAssemblyName_">The origin extension assembly name, given by Assembly.GetName().Name</param>
        /// <returns></returns>
        public static string GetPolicyName(string permissionCode_, string originExtensionAssemblyName_)
        {
            return $"{GetPermissionUniqueIdentifier(permissionCode_, originExtensionAssemblyName_)}".ToLowerInvariant();
        }

        /// <summary>
        /// Formats the unique identifier of a permission that has a code (name) and an origin extension.
        /// Note: the returned value is lower case.
        /// </summary>
        /// <param name="permissionCode_"></param>
        /// <param name="originExtensionAssemblyName_">The origin extension assembly name, given by Assembly.GetName().Name</param>
        /// <returns></returns>
        public static string GetPermissionUniqueIdentifier(string permissionCode_, string originExtensionAssemblyName_)
        {
            return
                $"{originExtensionAssemblyName_}.{permissionCode_}".ToLowerInvariant();
        }
    }
}
