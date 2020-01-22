// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX.
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Common.Enums;
using SoftinuxBase.Security.PermissionParts;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.Security.PermissionPartsTests
{
    public class DictionaryExtensionsTests
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

        #region UserHasThisPermission
        [Theory]
        [InlineData("SoftinuxBase.Security.PermissionParts", (short)Permissions.Read, true)]
        [InlineData("SoftinuxBase.Security.PermissionParts", (short)Permissions.Edit, false)]
        [InlineData("SoftinuxBase.Security.PermissionParts", (short)Permissions.AccessAll, false)]
        public void UserHasThisPermission(string typeFullName_, short permissionValue_, bool expectedFound_)
        {
            // Arrange
            PermissionsDictionary permissions = new PermissionsDictionary();
            permissions.Add(typeof(Permissions), (short)Permissions.Read);

            // Act
            var found = permissions.UserHasThisPermission(typeFullName_, permissionValue_);

            // Assert
            Assert.Equal(expectedFound_, found);
        }

        [Theory]
        [InlineData("SoftinuxBase.Security.PermissionParts", (short)Permissions.Read, true)]
        [InlineData("SoftinuxBase.Security.PermissionParts", (short)Permissions.Edit, true)]
        [InlineData("SoftinuxBase.Security.PermissionParts", (short)Permissions.AccessAll, true)]
        public void UserHasThisPermission_HasAccessAll(string typeFullName_, short permissionValue_, bool expectedFound_)
        {
            // Arrange
            PermissionsDictionary permissions = new PermissionsDictionary();
            permissions.Add(typeof(Permissions), (short)Permissions.Read);
            permissions.Add(typeof(Permissions), (short)Permissions.AccessAll);

            // Act
            var found = permissions.UserHasThisPermission(typeFullName_, permissionValue_);

            // Assert
            Assert.Equal(expectedFound_, found);
        }

        #endregion

        #region ThisPermissionIsAllowed

        [Theory]
        [InlineData(typeof(Permissions), (short)Permissions.Read, true)]
        [InlineData(typeof(Permissions), (short)Permissions.Edit, false)]
        [InlineData(typeof(Permissions), (short)Permissions.AccessAll, false)]
        public void ThisPermissionIsAllowed(Type type_, short permissionValue_, bool expectedAllowed_)
        {
            // Arrange
            PermissionsDictionary permissions = new PermissionsDictionary();
            permissions.Add(typeof(Permissions), (short)Permissions.Read);
            var packedPermissions = permissions.PackPermissions();

            // Act
            var allowed = packedPermissions.ThisPermissionIsAllowed(StringExtensions.ToPolicyName(type_, permissionValue_));

            // Assert
            Assert.Equal(expectedAllowed_, allowed);
        }

        [Theory]
        [InlineData(typeof(Permissions), (short)Permissions.Read, true)]
        [InlineData(typeof(Permissions), (short)Permissions.Edit, true)]
        [InlineData(typeof(Permissions), (short)Permissions.AccessAll, true)]
        public void ThisPermissionIsAllowed_HasAccessAll(Type type_, short permissionValue_, bool expectedAllowed_)
        {
            // Arrange
            PermissionsDictionary permissions = new PermissionsDictionary();
            permissions.Add(typeof(Permissions), (short)Permissions.AccessAll);
            var packedPermissions = permissions.PackPermissions();

            // Act
            var allowed = packedPermissions.ThisPermissionIsAllowed(StringExtensions.ToPolicyName(type_, permissionValue_));

            // Assert
            Assert.Equal(expectedAllowed_, allowed);
        }

        #endregion
    }
}
