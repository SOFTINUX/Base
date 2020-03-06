// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using FluentAssertions;
using SoftinuxBase.Security.Permissions;
using Xunit;
using Constants = SoftinuxBase.Tests.Common.Constants;

namespace SoftinuxBase.Security.PermissionsTests
{
    public class PermissionsDisplayDictionaryTest
    {
        [Fact]
        public void Ctor_Get()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);

            // Act
            var permissionsDisplayDictionary = new PermissionsDisplayDictionary(permissionsDictionary);

            // Assert
            permissionsDisplayDictionary.Get(Constants.SoftinuxBaseSecurityPermissionsAssemblyShortName, "Roles", "CanCreate").Should().NotBeNull();
            permissionsDisplayDictionary.Get(Constants.SoftinuxBaseSecurityPermissionsAssemblyShortName, (short)Permissions.Enums.Permissions.CreateRoles).Should().NotBeNull();
        }
    }
}
