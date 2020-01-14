// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using FluentAssertions;
using SoftinuxBase.Security.PermissionParts;
using Xunit;

namespace SoftinuxBase.Security.PermissionPartsTests
{
    public class PermissionPackersTest
    {
        #region PackPermissions
        [Fact]
        public void PackPermissions_OnePermission()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);

            // Act
            var packedDictionary = permissionsDictionary.PackPermissions();

            // Assert
            packedDictionary.Keys.Count.Should().Be(1);
            packedDictionary.ContainsKey("SoftinuxBase.Security.PermissionParts").Should().BeTrue();
            packedDictionary["SoftinuxBase.Security.PermissionParts"].Should().BeEquivalentTo($"{(char)Permissions.CreateRoles}");
        }

        [Fact]
        public void PackPermissions_TwoPermissions()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.DeleteRoles);

            // Act
            var packedDictionary = permissionsDictionary.PackPermissions();

            // Assert
            packedDictionary.Keys.Count.Should().Be(1);
            packedDictionary.ContainsKey("SoftinuxBase.Security.PermissionParts").Should().BeTrue();
            packedDictionary["SoftinuxBase.Security.PermissionParts"].Should().BeEquivalentTo($"{(char)Permissions.CreateRoles}{(char)Permissions.DeleteRoles}");
        }

        [Fact]
        public void PackPermissions_PermissionsFromSeveralEnums()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.DeleteRoles);
            permissionsDictionary.Add(typeof(OtherPermissions), (short)OtherPermissions.Read);

            // Act
            var packedDictionary = permissionsDictionary.PackPermissions();

            // Assert
            packedDictionary.Keys.Count.Should().Be(2);
            packedDictionary.ContainsKey("SoftinuxBase.Security.PermissionParts").Should().BeTrue();
            packedDictionary.ContainsKey("SoftinuxBase.Security.PermissionPartsTests").Should().BeTrue();
            packedDictionary["SoftinuxBase.Security.PermissionParts"].Should().BeEquivalentTo($"{(char)Permissions.CreateRoles}{(char)Permissions.DeleteRoles}");
            packedDictionary["SoftinuxBase.Security.PermissionPartsTests"].Should().BeEquivalentTo($"{(char)OtherPermissions.Read}");
        }

        #endregion PackPermissions

        #region UnpackPermissions

        [Fact]
        public void UnpackPermissions_OnePermission()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);
            var packedDictionary = permissionsDictionary.PackPermissions();

            // Act
            var unpackedPermissions = packedDictionary.UnpackPermissions();

            // Assert
            unpackedPermissions.Should().BeEquivalentTo(permissionsDictionary);
        }

        [Fact]
        public void UnpackPermissions_TwoPermissions()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.DeleteRoles);
            var packedDictionary = permissionsDictionary.PackPermissions();

            // Act
            var unpackedPermissions = packedDictionary.UnpackPermissions();

            // Assert
            unpackedPermissions.Should().BeEquivalentTo(permissionsDictionary);
        }

        [Fact]
        public void UnpackPermissions_PermissionsFromSeveralEnums()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions), (short)Permissions.DeleteRoles);
            permissionsDictionary.Add(typeof(OtherPermissions), (short)OtherPermissions.Read);
            var packedDictionary = permissionsDictionary.PackPermissions();

            // Act
            var unpackedPermissions = packedDictionary.UnpackPermissions();

            // Assert
            unpackedPermissions.Should().BeEquivalentTo(permissionsDictionary);
        }

        #endregion
    }
}
