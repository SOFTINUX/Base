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
        public void Add_OnePermission()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.PermissionParts").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.PermissionParts"].Should().BeEquivalentTo(new HashSet<short> { (short)Permissions.CreateRoles });
        }

        [Fact]
        public void Add_OnePermissionTwice()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.PermissionParts").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.PermissionParts"].Should().BeEquivalentTo(new HashSet<short> { (short)Permissions.CreateRoles });
        }

        [Fact]
        public void Add_TwoPermissions()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.EditRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.PermissionParts").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.PermissionParts"].Should().BeEquivalentTo(new HashSet<short> { (short)Permissions.CreateRoles, (short)Permissions.EditRoles });
        }

        [Fact]
        public void Add_PermissionsFromSeveralEnums()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(OtherPermissions), (short)OtherPermissions.Read);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(2);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.PermissionParts").Should().BeTrue();
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.PermissionPartsTests").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.PermissionParts"].Should().BeEquivalentTo(new HashSet<short> { (short)Permissions.CreateRoles });
            permissionsDictionary.Dictionary["SoftinuxBase.Security.PermissionPartsTests"].Should().BeEquivalentTo(new HashSet<short> { (short)OtherPermissions.Read });
        }

        internal enum OtherPermissions
        {
            Read,
            Write
        }

    }
}
