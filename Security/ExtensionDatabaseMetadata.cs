// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure.Interfaces;

namespace Security
{
    public class ExtensionDatabaseMetadata : IExtensionDatabaseMetadata
    {
        // this value will not change
        private const string _securityAssemblyName = "Security";

        private Tuple<string, string, string> _superAdminUserData =
            new Tuple<string, string, string>("Super", "Admin", "Super Administrator");

        private Tuple<string, string, string> _adminUserData =
            new Tuple<string, string, string>("Test", "Admin", "Administrator");

        private Tuple<string, string, string> _userUserData =
            new Tuple<string, string, string>("Test", "User", "User");

        private KeyValuePair<string, string> _credentialTypeData =
            new KeyValuePair<string, string>("email", "E-mail and password");

        private IStorage _storage;

        public uint Priority => 0;

    }
}
