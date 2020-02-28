// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using FluentAssertions;
using SoftinuxBase.Security.Permissions;
using Xunit;

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
            permissionsDisplayDictionary.Get(Constants.SoftinuxBaseSecurityPermissions, "Roles", "CanCreate").Should().NotBeNull();
            permissionsDisplayDictionary.Get(Constants.SoftinuxBaseSecurityPermissions, (short)Permissions.Enums.Permissions.CreateRoles).Should().NotBeNull();
        }
    }
}
