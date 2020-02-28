// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX.
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.Security.PermissionsTests
{
    public class DictionaryExtensionsTests
    {
        #region PackPermissions
        [Fact]
        public void PackPermissions_OnePermission()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);

            // Act
            var packedDictionary = permissionsDictionary.PackPermissions();

            // Assert
            packedDictionary.Keys.Count.Should().Be(1);
            packedDictionary.ContainsKey($"{Constants.SoftinuxBaseSecurityPermissions}.Permissions").Should().BeTrue();
            packedDictionary[$"{Constants.SoftinuxBaseSecurityPermissions}.Permissions"].Should().BeEquivalentTo($"{(char)Permissions.Enums.Permissions.CreateRoles}");
        }

        [Fact]
        public void PackPermissions_TwoPermissions()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.DeleteRoles);

            // Act
            var packedDictionary = permissionsDictionary.PackPermissions();

            // Assert
            packedDictionary.Keys.Count.Should().Be(1);
            packedDictionary.ContainsKey($"{Constants.SoftinuxBaseSecurityPermissions}.Permissions").Should().BeTrue();
            packedDictionary[$"{Constants.SoftinuxBaseSecurityPermissions}.Permissions"].Should().BeEquivalentTo($"{(char)Permissions.Enums.Permissions.CreateRoles}{(char)Permissions.Enums.Permissions.DeleteRoles}");
        }

        [Fact]
        public void PackPermissions_PermissionsFromSeveralEnums()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.DeleteRoles);
            permissionsDictionary.Add(typeof(OtherPermissions), (short)OtherPermissions.Read);

            // Act
            var packedDictionary = permissionsDictionary.PackPermissions();

            // Assert
            packedDictionary.Keys.Count.Should().Be(2);
            packedDictionary.ContainsKey($"{Constants.SoftinuxBaseSecurityPermissions}.Permissions").Should().BeTrue();
            packedDictionary.ContainsKey("SoftinuxBase.Tests.Common.OtherPermissions").Should().BeTrue();
            packedDictionary[$"{Constants.SoftinuxBaseSecurityPermissions}.Permissions"].Should().BeEquivalentTo($"{(char)Permissions.Enums.Permissions.CreateRoles}{(char)Permissions.Enums.Permissions.DeleteRoles}");
            packedDictionary["SoftinuxBase.Tests.Common.Permissions"].Should().BeEquivalentTo($"{(char)OtherPermissions.Read}");
        }

        #endregion PackPermissions

        #region UnpackPermissions

        [Fact]
        public void UnpackPermissions_OnePermission()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);
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
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.DeleteRoles);
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
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.DeleteRoles);
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
        [InlineData("SoftinuxBase.Security.Permissions.Permissions", (short)Permissions.Enums.Permissions.Read, true)]
        [InlineData("SoftinuxBase.Security.Permissions.Permissions", (short)Permissions.Enums.Permissions.Edit, false)]
        [InlineData("SoftinuxBase.Security.Permissions.Permissions", (short)Permissions.Enums.Permissions.AccessAll, false)]
        public void UserHasThisPermission(string typeFullName_, short permissionValue_, bool expectedFound_)
        {
            // Arrange
            PermissionsDictionary permissions = new PermissionsDictionary();
            permissions.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.Read);

            // Act
            var found = permissions.UserHasThisPermission(typeFullName_, permissionValue_);

            // Assert
            Assert.Equal(expectedFound_, found);
        }

        [Theory]
        [InlineData("SoftinuxBase.Security.Permissions.Permissions", (short)Permissions.Enums.Permissions.Read, true)]
        [InlineData("SoftinuxBase.Security.Permissions.Permissions", (short)Permissions.Enums.Permissions.Edit, true)]
        [InlineData("SoftinuxBase.Security.Permissions.Permissions", (short)Permissions.Enums.Permissions.AccessAll, true)]
        public void UserHasThisPermission_HasAccessAll(string typeFullName_, short permissionValue_, bool expectedFound_)
        {
            // Arrange
            PermissionsDictionary permissions = new PermissionsDictionary();
            permissions.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.Read);
            permissions.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.AccessAll);

            // Act
            var found = permissions.UserHasThisPermission(typeFullName_, permissionValue_);

            // Assert
            Assert.Equal(expectedFound_, found);
        }

        #endregion

        #region ThisPermissionIsAllowed

        [Theory]
        [InlineData(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.Read, true)]
        [InlineData(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.Edit, false)]
        [InlineData(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.AccessAll, false)]
        public void ThisPermissionIsAllowed(Type type_, short permissionValue_, bool expectedAllowed_)
        {
            // Arrange
            PermissionsDictionary permissions = new PermissionsDictionary();
            permissions.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.Read);
            var packedPermissions = permissions.PackPermissions();

            // Act
            var allowed = packedPermissions.ThisPermissionIsAllowed(StringExtensions.ToPolicyName(type_, permissionValue_));

            // Assert
            Assert.Equal(expectedAllowed_, allowed);
        }

        [Theory]
        [InlineData(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.Read, true)]
        [InlineData(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.Edit, true)]
        [InlineData(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.AccessAll, true)]
        public void ThisPermissionIsAllowed_HasAccessAll(Type type_, short permissionValue_, bool expectedAllowed_)
        {
            // Arrange
            PermissionsDictionary permissions = new PermissionsDictionary();
            permissions.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.AccessAll);
            var packedPermissions = permissions.PackPermissions();

            // Act
            var allowed = packedPermissions.ThisPermissionIsAllowed(StringExtensions.ToPolicyName(type_, permissionValue_));

            // Assert
            Assert.Equal(expectedAllowed_, allowed);
        }

        #endregion
    }
}
