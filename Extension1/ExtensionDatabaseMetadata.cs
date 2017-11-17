﻿// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure;
using Security.Common.Enums;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Extension1
{
    public class ExtensionDatabaseMetadata : IExtensionDatabaseMetadata
    {
        public uint Priority => 10;

        private const string _permissionCode = "admin";
        public IEnumerable<Tuple<string, string, bool>> PermissionCodeLabelAndFlags =>
            new[] {new Tuple<string, string, bool>(_permissionCode, "Extension 1 administration", true)};

        public IEnumerable<KeyValuePair<string, string>> RoleCodeAndLabels => null;

        public IEnumerable<KeyValuePair<string, string>> GroupCodeAndLabels => null;
        public void ConfigureLinks(IStorage storage_)
        {
            IPermissionRepository permRepo = storage_.GetRepository<IPermissionRepository>();
            Permission perm1 = permRepo.WithKeys(_permissionCode, "Extension1");

            IRolePermissionRepository rolePermRepo = storage_.GetRepository<IRolePermissionRepository>();

            // if links already exist, don't recreate them
            if(rolePermRepo.WithKeys((int) RoleId.AdministratorOwner, perm1.Id) != null)
                return;

            rolePermRepo.Create(new RolePermission { PermissionId = perm1.Id, RoleId = (int) RoleId.Administrator});
            rolePermRepo.Create(new RolePermission { PermissionId = perm1.Id, RoleId = (int) RoleId.AdministratorOwner});

        }
    }
}