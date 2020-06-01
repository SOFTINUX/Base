// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace SoftinuxBase.Security.Permissions
{
    /// <summary>
    /// Extensions to work with packed permissions string.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Unpack the permissions from compact storage format.
        /// </summary>
        /// <param name="packedPermissions_"></param>
        /// <returns></returns>
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
        /// Gets the elements of the policy name.
        /// </summary>
        /// <param name="policyName_">Policy name.</param>
        /// <returns>The type assembly-qualified fullname, the permission short value.</returns>
        public static (string type, short permission) ParsePolicyName(this string policyName_)
        {
            var splitted = policyName_.Split('|');
            return (type: splitted[0], permission: Int16.Parse(splitted[1]));
        }

        /// <summary>
        /// Gets The name of the policy associated to this permission: an unique identifier generated from the permission type and value.
        /// </summary>
        /// <param name="permissionEnumType_"></param>
        /// <param name="permission_"></param>
        /// <returns>Policy name.</returns>
        public static string ToPolicyName(Type permissionEnumType_, short permission_)
        {
            return $"{permissionEnumType_.AssemblyQualifiedName}|{permission_.ToString()}";
        }

        /// <summary>
        /// Converts the database string to the packed role to permissions representation.
        /// </summary>
        /// <param name="roleToPermissionsDatabaseString_"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToPackedPermissions(this string roleToPermissionsDatabaseString_)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(roleToPermissionsDatabaseString_);
        }

        /// <summary>
        /// Converts the (database) storage string to the role to permissions dictionary representation.
        /// </summary>
        /// <param name="roleToPermissionsStorageString_"></param>
        /// <returns></returns>
        public static PermissionsDictionary ToPermissions(this string roleToPermissionsStorageString_)
        {
            return roleToPermissionsStorageString_.ToPackedPermissions().UnpackPermissions();
        }

        /// <summary>
        /// Debug helper to format a readable string from packed permissions claim value.
        /// </summary>
        /// <param name="packedPermissionsClaimValue_"></param>
        /// <returns></returns>
        public static string ToDisplayString(this string packedPermissionsClaimValue_)
        {
            var permissionDictionary = packedPermissionsClaimValue_.ToPermissions();
            StringBuilder sb = new StringBuilder();
            foreach (var key in permissionDictionary.Dictionary.Keys)
            {
                sb.Append("[[").Append(key).Append("] ");
                foreach (var value in permissionDictionary.Dictionary[key])
                {
                    sb.Append(value).Append(" ");
                }

                sb.Append("] ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get the assembly short name, i.e. "MyAssembly" from a type assembly-qualified name, i.e. "MyAssembly.MyNamespace.MyType, MyAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null".
        /// </summary>
        /// <param name="typeAssemblyQualifiedName_"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When the string is not a type name</exception>
        public static string GetAssemblyShortName(this string typeAssemblyQualifiedName_)
        {
            if (typeAssemblyQualifiedName_.IndexOf(',') == -1)
            {
                throw new ArgumentException($"'{typeAssemblyQualifiedName_}' is not a type assembly-qualified name", nameof(typeAssemblyQualifiedName_));
            }

            var assemblyName = new AssemblyName(typeAssemblyQualifiedName_.Substring(typeAssemblyQualifiedName_.IndexOf(',') + 1));
            return assemblyName.Name;
        }

        /// <summary>
        /// Transform a label to a string suitable for an html ID.
        /// </summary>
        public static string ToId(this string label_) { return label_.ToLowerInvariant().Replace('.', '-').Replace(' ', '-'); }
    }
}