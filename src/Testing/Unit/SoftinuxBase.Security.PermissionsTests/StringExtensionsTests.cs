using FluentAssertions;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.Security.PermissionsTests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ToDisplayString()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();
            permissionsDictionary.Add(typeof(Common.Enums.Permissions), (short)Common.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Common.Enums.Permissions), (short)Common.Enums.Permissions.DeleteRoles);
            permissionsDictionary.Add(typeof(OtherPermissions), (short)OtherPermissions.Read);

            // Act
            var displayString = permissionsDictionary.PackPermissions().ToPackedString().ToDisplayString();

            // Assert
            displayString.Should().BeEquivalentTo("[[SoftinuxBase.Security.Common] 22 24 ] [[SoftinuxBase.Tests.Common] 0 ] ");
        }
    }
}
