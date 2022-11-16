// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using FluentAssertions;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Tests.Common;
using Xunit;
using Constants = SoftinuxBase.Tests.Common.Constants;

namespace SoftinuxBase.Security.PermissionsTests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ToDisplayString()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.DeleteRoles);
            permissionsDictionary.Add(typeof(OtherPermissions), (short)OtherPermissions.Read);

            // Act
            var displayString = permissionsDictionary.PackPermissions().ToStorageString().ToDisplayString();

            // Assert
            displayString.Should().BeEquivalentTo($"[[{Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName}] 22 24 ] [[{Constants.SoftinuxBaseTestsCommonOtherPermissionsEnumAssemblyQualifiedName}] 0 ] ");
        }

        [Fact]
        public void GetAssemblyShortName()
        {
            // Arrange
            var enumTypeAssemblyQualifiedName = Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName;
            enumTypeAssemblyQualifiedName.IndexOf(',').Should().NotBe(-1);

            // Act
            var assemblyShortName = enumTypeAssemblyQualifiedName.GetAssemblyShortName();

            // Assert
            assemblyShortName.Should().Be(Constants.SoftinuxBaseSecurityPermissionsAssemblyShortName);
        }
    }
}