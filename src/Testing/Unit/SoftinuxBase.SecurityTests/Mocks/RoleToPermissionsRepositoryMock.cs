// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Moq;
using SampleExtension1;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Tests.Common;

namespace SoftinuxBase.SecurityTests.Mocks
{
    public class RoleToPermissionsRepositoryMock : Mock<IRoleToPermissionsRepository>
    {
        public RoleToPermissionsRepositoryMock()
        {
            Setup();
        }

        private void Setup()
        {
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Security.Permissions.Enums.Permissions), (short)SoftinuxBase.Security.Permissions.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Security.Permissions.Enums.Permissions), (short)SoftinuxBase.Security.Permissions.Enums.Permissions.DeleteRoles);
            permissionsDictionary.Add(typeof(OtherPermissions), (short)OtherPermissions.Write);
            permissionsDictionary.Add(typeof(SamplePermissions), (short)SamplePermissions.Admin);
            var administratorPermissions = new RoleToPermissions(Roles.Administrator.ToString(), Roles.Administrator.ToString(), permissionsDictionary);

            permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Security.Permissions.Enums.Permissions), (short)SoftinuxBase.Security.Permissions.Enums.Permissions.ListRoles);
            permissionsDictionary.Add(typeof(Security.Permissions.Enums.Permissions), (short)SoftinuxBase.Security.Permissions.Enums.Permissions.ReadRoles);
            permissionsDictionary.Add(typeof(OtherPermissions), (short)OtherPermissions.Read);
            permissionsDictionary.Add(typeof(SamplePermissions), (short)SamplePermissions.Write);
            permissionsDictionary.Add(typeof(SamplePermissions), (short)SamplePermissions.Read);
            permissionsDictionary.Add(typeof(SamplePermissions), (short)SamplePermissions.Other);
            var moderatorPermissions = new RoleToPermissions(Roles.Moderator.ToString(), Roles.Moderator.ToString(), permissionsDictionary);

            Setup(m => m.All()).Returns(new List<RoleToPermissions> {administratorPermissions, moderatorPermissions});
        }
    }
}