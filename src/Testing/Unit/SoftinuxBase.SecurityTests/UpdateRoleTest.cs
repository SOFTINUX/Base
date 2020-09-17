// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Tools;
using Xunit;

namespace SoftinuxBase.SecurityTests
{
    public class UpdateRoleTest
    {
        [Fact]
        public async Task IsRoleNameAvailableAsync_True_NoRoleByThisName()
        {
            // Arrange
            var roleManager = new Mock<IAspNetRolesManager>();
            var roleName = Guid.NewGuid().ToString();
            var roleId = Guid.NewGuid().ToString();
            roleManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(default(IdentityRole<string>));

            // Act
            var result = await UpdateRole.IsRoleNameAvailableAsync(roleManager.Object, roleName, roleId);

            // Assert
            result.Should().BeTrue();
            roleManager.Verify(m => m.FindByNameAsync(roleName), Times.Once);
        }

        [Fact]
        public async Task IsRoleNameAvailableAsync_True_RoleByThisNameSameId()
        {
            // Arrange
            var roleManager = new Mock<IAspNetRolesManager>();
            var roleName = Guid.NewGuid().ToString();
            var roleId = Guid.NewGuid().ToString();
            var role = new IdentityRole<string> { Id = roleId, Name = roleName };
            roleManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);

            // Act
            var result = await UpdateRole.IsRoleNameAvailableAsync(roleManager.Object, roleName, roleId);

            // Assert
            result.Should().BeTrue();
            roleManager.Verify(m => m.FindByNameAsync(roleName), Times.Once);
        }

        [Fact]
        public async Task IsRoleNameAvailableAsync_False_RoleByThisNameOtherId()
        {
            // Arrange
            var roleManager = new Mock<IAspNetRolesManager>();
            var roleName = Guid.NewGuid().ToString();
            var roleId = Guid.NewGuid().ToString();
            var otherRoleId = Guid.NewGuid().ToString();
            var role = new IdentityRole<string> { Id = otherRoleId, Name = roleName };
            roleManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);

            // Act
            var result = await UpdateRole.IsRoleNameAvailableAsync(roleManager.Object, roleName, roleId);

            // Assert
            result.Should().BeFalse();
            roleManager.Verify(m => m.FindByNameAsync(roleName), Times.Once);
        }
    }
}