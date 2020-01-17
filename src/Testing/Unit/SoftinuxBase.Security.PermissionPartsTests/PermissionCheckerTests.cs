using System;
using SoftinuxBase.Security.PermissionParts;
using Xunit;

namespace SoftinuxBase.Security.PermissionPartsTests
{
    public class PermissionCheckerTests
    {
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
