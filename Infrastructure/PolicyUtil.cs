namespace Infrastructure
{
    public static class PolicyUtil
    {
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
            return $"{permissionCode_}|{typeof(T).Assembly.FullName}|{(write_ ? READ_WRITE_SUFFIX : READ_ONLY_SUFFIX)}";
        }

        /// <summary>
        /// Formats the unique identifier of a permission that has a code and an origin extension.
        /// </summary>
        /// <param name="permissionCode_"></param>
        /// <param name="originExtensionAssemblyFullName_"></param>
        /// <returns></returns>
        public static string GetPermissionUniqueIdentifier(string permissionCode_, string originExtensionAssemblyFullName_)
        {
            return
                $"{permissionCode_}|{originExtensionAssemblyFullName_}";
        }
    }
}
