// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure;
using Security.Enums;

namespace Security
{
    public class ExtensionDatabaseMetadata : IExtensionDatabaseMetadata
    {
        public IEnumerable<Tuple<string, string, bool>> PermissionCodeLabelAndFlags =>
            new[]
            {
                new Tuple<string, string, bool>(Permission.PERM_CODE_EDIT_PERMISSION, "Permissions management", true),
                new Tuple<string, string, bool>(Permission.PERM_CODE_EDIT_ROLE, "Roles management", true),
                new Tuple<string, string, bool>(Permission.PERM_CODE_EDIT_USER, "Users management", true),
                new Tuple<string, string, bool>(Permission.PERM_CODE_EDIT_GROUP, "Groups management", true)
            };

        public IEnumerable<KeyValuePair<string, string>> RoleCodeAndLabels =>
                   new[]
                   {
                new KeyValuePair<string, string>("administrator-owner", "Administrator Owner"),
                new KeyValuePair<string, string>("administrator", "Administrator"),
                new KeyValuePair<string, string>("user", "Administrator User"),
                   };

        public IEnumerable<KeyValuePair<string, string>> GroupCodeAndLabels => null;

        public void ConfigureLinks(IStorage storage_)
        {
            new DatabaseInitializer().CheckAndInitialize(storage_);
        }
    }
}
