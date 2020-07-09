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
        public void Add_Get_NotNull()
        {
            // Arrange
            var permissionType = typeof(Permissions.Enums.Permissions);
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(permissionType, (short)Permissions.Enums.Permissions.CreateRoles);

            // Act
            var permissionsDisplayDictionary = new PermissionsDisplayDictionary();
            permissionsDisplayDictionary.Add(Constants.SoftinuxBaseSecurityPermissionsAssemblyShortName, permissionType);

            // Assert
            permissionsDisplayDictionary.Get(Constants.SoftinuxBaseSecurityPermissionsAssemblyShortName, "Role management", "CanCreate").Should().NotBeNull();
            permissionsDisplayDictionary.Get(Constants.SoftinuxBaseSecurityPermissionsAssemblyShortName, (short)Permissions.Enums.Permissions.CreateRoles).Should().NotBeNull();
        }

        [Fact]
        public void Add_Get_Null()
        {
            // Arrange
            var permissionType = typeof(Permissions.Enums.Permissions);
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(permissionType, (short)Permissions.Enums.Permissions.CreateRoles);

            // Act
            var permissionsDisplayDictionary = new PermissionsDisplayDictionary();
            permissionsDisplayDictionary.Add(Constants.SoftinuxBaseSecurityPermissionsAssemblyShortName, permissionType);

            // Assert
            // extension name not in permissionsDisplayDictionary
            permissionsDisplayDictionary.Get("OtherExtension", "Roles", "CanCreate").Should().BeNull();
            // Permission value not in permissionsDisplayDictionary
            permissionsDisplayDictionary.Get(Constants.SoftinuxBaseSecurityPermissionsAssemblyShortName, (short)Permissions.Enums.Permissions.DeleteRoles).Should().BeNull();
        }
    }
}
