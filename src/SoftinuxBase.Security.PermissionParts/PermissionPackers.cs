// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftinuxBase.Security.PermissionParts
{
    public static class PermissionPackers
    {
        // to be removed
        public static string PackPermissionsIntoString(this IEnumerable<Permissions> permissions)
        {
            return permissions.Aggregate("", (s, permission) => s + (char)permission);
        }

        // to be removed
        public static IEnumerable<Permissions> UnpackPermissionsFromString(this string packedPermissions)
        {
            if (packedPermissions == null)
                throw new ArgumentNullException(nameof(packedPermissions));
            foreach (var character in packedPermissions)
            {
                yield return ((Permissions)character);
            }
        }

        //public static Permissions? FindPermissionViaName(this string permissionName)
        //{
        //    return Enum.TryParse(permissionName, out Permissions permission)
        //        ? (Permissions?)permission
        //        : null;
        //}

        public static string PackPermissions(this IEnumerable<short> permissions_)
        {
            return permissions_.Aggregate("", (s, permission) => s + (char)permission);
        }

        public static IEnumerable<short> UnpackPermissions(this string packedPermissions_)
        {
            if (packedPermissions_ == null)
                throw new ArgumentNullException(nameof(packedPermissions_));
            foreach (var character in packedPermissions_)
            {
                yield return ((short)character);
            }
        }

        //TOTEST
        /// <summary>
        /// Pack permissions to compact format.
        /// </summary>
        /// <param name="dictionary_"></param>
        /// <returns></returns>
        public static Dictionary<string, string> PackPermissionsDictionary(this PermissionsDictionary dictionary_)
        {
            var packedDictionary = new Dictionary<string, string>();
            foreach (var key in dictionary_.Dictionary.Keys)
            {
                packedDictionary.Add(key, dictionary_.Dictionary[key].PackPermissions());
            }
            return packedDictionary;
        }

        //TOTEST
        /// <summary>
        /// Unpack permissions to human readable format.
        /// </summary>
        /// <param name="dictionary_"></param>
        /// <returns></returns>
        public static PermissionsDictionary UnpackPermissionsDictionary(this Dictionary<string, string> dictionary_)
        {
            var unpackedDictionary = new PermissionsDictionary();
            foreach (var values in dictionary_.Values)
            {
                unpackedDictionary.AddGrouped(values.UnpackPermissions());
            }
            return unpackedDictionary;
        }

    }
}