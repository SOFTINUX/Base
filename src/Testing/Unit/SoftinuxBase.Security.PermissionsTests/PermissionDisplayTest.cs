// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SoftinuxBase.Security.Permissions;
using Xunit;

namespace SoftinuxBase.Security.PermissionsTests
{
    public class PermissionDisplayTest
    {
        [Fact]
        public void GetPermissionsToDisplay()
        {
            // Arrange
            Type permissionsEnum = typeof(Permissions.Enums.Permissions);

            // Act
            var permissionsToDisplay = PermissionDisplay.GetPermissionsToDisplay(permissionsEnum);

            // Assert
            permissionsToDisplay.Should().NotBeEmpty();
            permissionsToDisplay.FirstOrDefault(p => p.GroupName == "Roles" && p.ShortName == "CanCreate").Should().NotBeNull();
            permissionsToDisplay.FirstOrDefault(p => p.GroupName == "Users" && p.ShortName == "CanRead").Should().NotBeNull();
        }

        [Fact]
        public void GetPermissionsToDisplay_WithOptionalValues()
        {
            // Arrange
            Type permissionsEnum = typeof(Permissions.Enums.Permissions);
            HashSet<short> values = new HashSet<short>();
            values.Add((short)Permissions.Enums.Permissions.CreateRoles);

            // Act
            var permissionsToDisplay = PermissionDisplay.GetPermissionsToDisplay(permissionsEnum, values);

            // Assert
            permissionsToDisplay.Should().NotBeEmpty();
            permissionsToDisplay.FirstOrDefault(p => p.GroupName == "Roles" && p.ShortName == "CanCreate").Should().NotBeNull();
            permissionsToDisplay.FirstOrDefault(p => p.GroupName == "Users" && p.ShortName == "CanRead").Should().BeNull();
        }
    }
}
