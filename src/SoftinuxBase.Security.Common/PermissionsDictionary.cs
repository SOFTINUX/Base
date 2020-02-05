﻿// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using SoftinuxBase.Security.Common.Enums;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Permissions")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.CommonTests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.PermissionsTests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SeedDatabase")]
namespace SoftinuxBase.Security.Common
{
    /// <summary>
    /// Permissions sorted by extension. This is used to represent role's or user's permissions.
    /// </summary>
    public class PermissionsDictionary
    {
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
                foreach (var enumTypeFullName in dictionary.Dictionary.Keys)
                {
                    merged.AddGrouped(enumTypeFullName, dictionary.Dictionary[enumTypeFullName]);
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
            var typeName = permissionEnumType_.FullName;
            Dictionary.TryGetValue(typeName, out var permissions);
            if (permissions == null)
            {
                permissions = new HashSet<short>();
                Dictionary.Add(typeName, permissions);
            }
            permissions.Add(permission_);
        }

        /// <summary>
        /// Add a group of permissions.
        /// </summary>
        /// <param name="permissionEnumTypeFullName_">Type full name</param>
        /// <param name="permissions_">Any value of enum defined in <paramref name="permissionEnumTypeFullName_"/> extension</param>
        internal void AddGrouped(string permissionEnumTypeFullName_, IEnumerable<short> permissions_)
        {
            Dictionary.TryGetValue(permissionEnumTypeFullName_, out var permissions);
            if (permissions == null)
            {
                permissions = new HashSet<short>();
                Dictionary.Add(permissionEnumTypeFullName_, permissions);
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
        /// <param name="permissionToCheckEnumTypeFullName_">Fullname of the permission's enum Type.</param>
        /// <param name="permissionToCheck_">Permission value.</param>
        /// <returns>true when the item is found.</returns>
        internal bool Contains(string permissionToCheckEnumTypeFullName_, short permissionToCheck_)
        {
            if (Dictionary.ContainsKey(permissionToCheckEnumTypeFullName_) && Dictionary[permissionToCheckEnumTypeFullName_].Contains(permissionToCheck_))
            {
                return true;
            }

            var key2 = typeof(Permissions).FullName;
            return Dictionary.ContainsKey(key2) && Dictionary[key2].Contains((short)Permissions.AccessAll);
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