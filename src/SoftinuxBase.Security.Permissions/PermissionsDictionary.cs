// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.PermissionsTests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SeedDatabase")]

namespace SoftinuxBase.Security.Permissions
{
    /// <summary>
    /// Permissions sorted by extension. This is used to represent role's or user's permissions.
    /// This is data/storage oriented.
    /// <seealso cref="PermissionsDisplayDictionary"/>.
    /// </summary>
    public class PermissionsDictionary
    {
        /// <summary>
        /// Internal dictionary which key is the extension permission enum type assembly-qualified fullname and the values some values (linked to role/user) of this enum.
        /// </summary>
        internal readonly Dictionary<string, HashSet<short>> Dictionary = new Dictionary<string, HashSet<short>>();

        /// <summary>
        /// Merge several dictionaries (for example one per role) to a single one (all user's permissions).
        /// </summary>
        /// <param name="dictionaries_">PermissionDictionaries</param>
        /// <returns>Merged permissionsDictionary</returns>
        public static PermissionsDictionary Merge(params PermissionsDictionary[] dictionaries_)
        {
            PermissionsDictionary merged = new PermissionsDictionary();
            foreach (var dictionary in dictionaries_)
            {
                foreach (var enumTypeAssemblyQualifiedFullName in dictionary.Dictionary.Keys)
                {
                    merged.AddGrouped(enumTypeAssemblyQualifiedFullName, dictionary.Dictionary[enumTypeAssemblyQualifiedFullName]);
                }
            }

            return merged;
        }

        /// <summary>
        /// Add a permission if it doesn't already exist.
        /// </summary>
        /// <param name="permissionEnumType_">The enum type</param>
        /// <param name="permission_">Any value of <paramref name="permissionEnumType_"/></param>
        public void Add(Type permissionEnumType_, short permission_)
        {
            var assemblyQualifiedTypeName = permissionEnumType_.AssemblyQualifiedName;
            Dictionary.TryGetValue(assemblyQualifiedTypeName, out var permissions);
            if (permissions == null)
            {
                permissions = new HashSet<short>();
                Dictionary.Add(assemblyQualifiedTypeName, permissions);
            }

            permissions.Add(permission_);
        }

        /// <summary>
        /// Add a group of permissions.
        /// </summary>
        /// <param name="permissionEnumAssemblyQualifiedTypeName_">Assembly-qualified type full name</param>
        /// <param name="permissions_">Any value of enum defined in <paramref name="permissionEnumAssemblyQualifiedTypeName_"/> extension</param>
        internal void AddGrouped(string permissionEnumAssemblyQualifiedTypeName_, IEnumerable<short> permissions_)
        {
            // TODO change the value the caller should pass as first arg
            Dictionary.TryGetValue(permissionEnumAssemblyQualifiedTypeName_, out var permissions);
            if (permissions == null)
            {
                permissions = new HashSet<short>();
                Dictionary.Add(permissionEnumAssemblyQualifiedTypeName_, permissions);
            }

            foreach (var permission in permissions_)
            {
                permissions.Add(permission);
            }
        }

        /// <summary>
        /// Check whether the item defined by a type and a short value is present.
        /// Or the <see cref="Permissions.AccessAll"/> permission is present.
        /// </summary>
        /// <param name="permissionToCheckAssemblyQualifiedEnumTypeFullName_">Fullname of the permission's enum Type.</param>
        /// <param name="permissionToCheck_">Permission value.</param>
        /// <returns>true when the item is found.</returns>
        internal bool Contains(string permissionToCheckAssemblyQualifiedEnumTypeFullName_, short permissionToCheck_)
        {
            if (Dictionary.ContainsKey(permissionToCheckAssemblyQualifiedEnumTypeFullName_) && Dictionary[permissionToCheckAssemblyQualifiedEnumTypeFullName_].Contains(permissionToCheck_))
            {
                return true;
            }

            var key2 = typeof(Enums.Permissions).AssemblyQualifiedName;
            if (String.IsNullOrEmpty(key2))
                return false;
            return Dictionary.ContainsKey(key2) && Dictionary[key2].Contains((short)Enums.Permissions.AccessAll);
        }

        /// <summary>
        /// Check whether there is a permission item present.
        /// </summary>
        /// <returns>true when an item is present.</returns>
        public bool Any()
        {
            if (Dictionary.Keys.Count == 0)
            {
                return false;
            }

            return Dictionary[Dictionary.Keys.First()].Count != 0;
        }
    }
}