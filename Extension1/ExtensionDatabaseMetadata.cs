// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Extension1
{
    public class ExtensionDatabaseMetadata : IExtensionDatabaseMetadata
    {
        public IEnumerable<Tuple<string, string, bool>> PermissionCodeLabelAndFlags =>
            new[] {new Tuple<string, string, bool>("admin", "Extension 1 administration", true)};

        public IEnumerable<KeyValuePair<string, string>> RoleCodeAndLabels => null;

        public IEnumerable<KeyValuePair<string, string>> GroupCodeAndLabels => null;
        public void ConfigureLinks(IStorage storage_)
        {
            //Permission extensionPermission = storage_.GetRepository<IPermissionRepository>().w
            // TODO add a WithKeys(code, extensionAssemblyShortName:string) method to find the permission, then use it below
            // with roles "admin" and "admin-owner".
            IRolePermissionRepository repo = storage_.GetRepository<IRolePermissionRepository>();
            //repo.Create(new RolePermission);
        }
    }
}
