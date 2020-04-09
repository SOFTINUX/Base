// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX.
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SoftinuxBase.Security.Permissions
{
    /// <summary>
    /// Extensions to work with packed and unpacked permissions dictionary.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Pack permissions to compact format.
        /// </summary>
        /// <param name="dictionary_">Permissions.</param>
        /// <returns>Packed permissions dictionary.</returns>
        public static Dictionary<string, string> PackPermissions(this PermissionsDictionary dictionary_)
        {
            var packedDictionary = new Dictionary<string, string>();
            foreach (var key in dictionary_.Dictionary.Keys)
            {
                packedDictionary.Add(key, dictionary_.Dictionary[key].PackPermissions());
            }
            return packedDictionary;
        }

        /// <summary>
        /// Format packed permissions to storage string.
        /// </summary>
        /// <param name="dictionary_">Packed permissions dictionary.</param>
        /// <returns>JSON string</returns>
        public static string ToStorageString(this Dictionary<string, string> dictionary_)
        {
            return JsonConvert.SerializeObject(dictionary_);
        }

        /// <summary>
        /// Create a string from short values to gain storage space.
        /// </summary>
        /// <param name="permissions_">Permissions values.</param>
        /// <returns>String.</returns>
        private static string PackPermissions(this IEnumerable<short> permissions_)
        {
            return permissions_.Aggregate("", (s_, permission_) => s_ + (char)permission_);
        }


        /// <summary>
        /// Unpack permissions to human readable format.
        /// </summary>
        /// <param name="dictionary_">Packed permissions dictionary.</param>
        /// <returns>Friendly permissions dictionary.</returns>
        public static PermissionsDictionary UnpackPermissions(this Dictionary<string, string> dictionary_)
        {
            var unpackedDictionary = new PermissionsDictionary();
            foreach (var key in dictionary_.Keys)
            {
                unpackedDictionary.AddGrouped(key, dictionary_[key].UnpackPermissions());
            }
            return unpackedDictionary;
        }

        /// <summary>
        /// Transform the permission dictionary to data suitable for display and admin management.
        /// </summary>
        /// <param name="permissionsDictionary_"></param>
        /// <returns></returns>
        public static Dictionary<string, List<PermissionDisplay>> ToDisplay(this PermissionsDictionary permissionsDictionary_)
        {
            // TODO
            return null;
        }

        /// <summary>
        /// This is used by the policy provider to check the permission name string.
        /// </summary>
        /// <param name="packedPermissions_">Packed permissions dictionary.</param>
        /// <param name="permissionName_">String that identifies the permission (policy).</param>
        /// <returns>True when the policy is found.</returns>
        public static bool ThisPermissionIsAllowed(this Dictionary<string, string> packedPermissions_, string permissionName_)
        {
            var usersPermissions = packedPermissions_.UnpackPermissions();
            var nameParts = permissionName_.ParsePolicyName();
            return UserHasThisPermission(usersPermissions, nameParts.type, nameParts.permission);
        }

        /// <summary>
        /// This is the main checker of whether a user permissions allows them to access something with the given permission.
        /// </summary>
        /// <param name="usersPermissions_">The permissions dictionary of the user.</param>
        /// <param name="permissionToCheckEnumTypeFullName_">The fullname of the type of the permissions enum.</param>
        /// <param name="permissionToCheck_">The permission value.</param>
        /// <returns>True when the user has the permission.</returns>
        public static bool UserHasThisPermission(this PermissionsDictionary usersPermissions_, string permissionToCheckEnumTypeFullName_, short permissionToCheck_)
        {
            return usersPermissions_.Contains(permissionToCheckEnumTypeFullName_, permissionToCheck_);
        }
    }
}
