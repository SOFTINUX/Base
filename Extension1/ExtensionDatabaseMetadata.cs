// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure;
using Infrastructure.Enums;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Extension1
{
    public class ExtensionDatabaseMetadata : ExtensionDatabaseMetadataBase
    {
        public override uint Priority => 10;

        private const string _permissionCode = "admin";
        //public override IEnumerable<Tuple<string, string, bool>> PermissionCodeLabelAndFlags =>
        //    new[] {new Tuple<string, string, bool>(_permissionCode, "Extension 1 administration", true)};

        //public override IEnumerable<KeyValuePair<string, string>> RoleCodeAndLabels => null;

        //public override IEnumerable<KeyValuePair<string, string>> GroupCodeAndLabels => null;

    }
}
