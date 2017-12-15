// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure;
using Infrastructure.Enums;
using Security.Common.Enums;
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

        public override void ConfigureLinks(IStorage storage_)
        {
        //    IPermissionRepository permRepo = storage_.GetRepository<IPermissionRepository>();
        //    Permission perm1 = permRepo.FindBy(_permissionCode, "Extension1");

        //    IRolePermissionRepository rolePermRepo = storage_.GetRepository<IRolePermissionRepository>();

        //    IRoleRepository roleRepo = storage_.GetRepository<IRoleRepository>();
        //    int adminOwnerRoleId = roleRepo.FindBy(Roles.ROLE_CODE_ADMINISTRATOR_OWNER, "Security").Id; // TODO use a constant or another method rather than "Security" hardcoded string
        //    int adminRoleId = roleRepo.FindBy(Roles.ROLE_CODE_ADMINISTRATOR, "Security").Id;

        //    // if links already exist, don't recreate them
        //    if (rolePermRepo.FindBy(adminOwnerRoleId, perm1.Id) != null)
        //        return;

        //    // Get the permission level we need
        //    IPermissionLevelRepository levelRepo = storage_.GetRepository<IPermissionLevelRepository>();
        //    PermissionLevel writeLevel = levelRepo.FindBy(PermissionLevelValue.ReadWrite);

        //    // Create the links
        //    rolePermRepo.Create(new RolePermission { PermissionId = perm1.Id, RoleId = adminRoleId, PermissionLevelId = writeLevel.Id});
        //    rolePermRepo.Create(new RolePermission { PermissionId = perm1.Id, RoleId = adminOwnerRoleId, PermissionLevelId = writeLevel.Id });

        }
    }
}
