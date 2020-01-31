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
            var displayString = permissionsDictionary.PackPermissions().ToStorageString().ToDisplayString();

            // Assert
            displayString.Should().BeEquivalentTo("[[SoftinuxBase.Security.Common.Permissions] 22 24 ] [[SoftinuxBase.Tests.Common.OtherPermissions] 0 ] ");

            
        }
    }
}
