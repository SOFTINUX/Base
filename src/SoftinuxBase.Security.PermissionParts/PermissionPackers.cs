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
        public static IEnumerable<Permissions> UnpackPermissionsFromString(this string packedPermissions)
        {
            if (packedPermissions == null)
                throw new ArgumentNullException(nameof(packedPermissions));
            foreach (var character in packedPermissions)
            {
                yield return ((Permissions)character);
            }
        }

        // to be removed
        public static string PackPermissions(this IEnumerable<Permissions> permissions_)
        {
            return permissions_.Aggregate("", (s, permission) => s + (char)permission);
        }

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

        /// <summary>
        /// Pack permissions to compact format.
        /// </summary>
        /// <param name="dictionary_"></param>
        /// <returns></returns>
        public static Dictionary<string, string> PackPermissions(this PermissionsDictionary dictionary_)
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
        public static PermissionsDictionary UnpackPermissions(this Dictionary<string, string> dictionary_)
        {
            var unpackedDictionary = new PermissionsDictionary();
            foreach (var key in dictionary_.Keys)
            {
                unpackedDictionary.AddGrouped(key, dictionary_[key].UnpackPermissions());
            }
            return unpackedDictionary;
        }

    }
}