// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using SoftinuxBase.Security.PermissionParts;
using Xunit;

namespace SoftinuxBase.Security.PermissionPartsTests
{
    public class PermissionsDictionaryTest
    {
        [Fact]
        public void AddOnePermission()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(Permissions.CreateRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.PermissionParts").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.PermissionParts"].Should().BeEquivalentTo(new HashSet<short> { (short)Permissions.CreateRoles });
        }

        [Fact]
        public void AddOnePermissionTwice()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(Permissions.CreateRoles);
            permissionsDictionary.Add(Permissions.CreateRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.PermissionParts").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.PermissionParts"].Should().BeEquivalentTo(new HashSet<short> { (short)Permissions.CreateRoles });
        }

        [Fact]
        public void AddTwoPermissions()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(Permissions.CreateRoles);
            permissionsDictionary.Add(Permissions.EditRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.PermissionParts").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.PermissionParts"].Should().BeEquivalentTo(new HashSet<short> { (short)Permissions.CreateRoles, (short)Permissions.EditRoles });
        }

    }
}
