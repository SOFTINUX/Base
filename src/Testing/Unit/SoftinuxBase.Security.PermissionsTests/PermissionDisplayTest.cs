﻿// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.Security.PermissionsTests
{
    public class PermissionDisplayTest
    {
        [Fact]
        public void GetPermissionsToDisplay_SecurityPermissions()
        {
            // Arrange
            Type permissionsEnum = typeof(Permissions.Enums.Permissions);

            // Act
            var permissionsToDisplay = PermissionDisplay.GetPermissionsToDisplay(Constants.SoftinuxBaseSecurityAssemblyShortName, permissionsEnum);

            // Assert
            permissionsToDisplay.Should().NotBeEmpty();
            permissionsToDisplay.FirstOrDefault(p => p.Section == "Role management" && p.ShortName == "CanCreate").Should().NotBeNull();
            permissionsToDisplay.FirstOrDefault(p => p.Section == "User management" && p.ShortName == "CanRead").Should().NotBeNull();
        }
        
        [Fact]
        public void GetPermissionsToDisplay_SampleExtension1Permissions()
        {
            // Arrange
            Type permissionsEnum = typeof(SampleExtension1.SamplePermissions);

            // Act
            var permissionsToDisplay = PermissionDisplay.GetPermissionsToDisplay("SampleExtension1",permissionsEnum);

            // Assert
            permissionsToDisplay.Should().NotBeEmpty();
        }

        [Fact]
        public void GetPermissionsToDisplay_WithOptionalValues()
        {
            // Arrange
            Type permissionsEnum = typeof(Permissions.Enums.Permissions);
            HashSet<short> values = new HashSet<short>();
            values.Add((short)Permissions.Enums.Permissions.CreateRoles);

            // Act
            var permissionsToDisplay = PermissionDisplay.GetPermissionsToDisplay("Any", permissionsEnum, values);

            // Assert
            permissionsToDisplay.Should().NotBeEmpty();
            permissionsToDisplay.FirstOrDefault(p => p.Section == "Role management" && p.ShortName == "CanCreate").Should().NotBeNull();
            permissionsToDisplay.FirstOrDefault(p => p.Section == "User management" && p.ShortName == "CanRead").Should().BeNull();
        }
    }
}
