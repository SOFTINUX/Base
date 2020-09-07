// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.Security.ViewModels.Permissions;
using Xunit;

namespace SoftinuxBase.SecurityTests
{
    public class CreateRoleTest
    {
        [Fact]
        public async Task CheckAndSaveNewRoleAsync_Ok()
        {
            // Arrange
            var newRoleName = "New Role 1";
            var mockAspNetRolesManager = new Mock<IAspNetRolesManager>();

            // Act
            var errorMessage = await CreateRole.CheckAndSaveNewRoleAsync(mockAspNetRolesManager.Object, new SaveNewRoleViewModel {RoleName = newRoleName});

            // Assert
            errorMessage.Should().BeNull();
            mockAspNetRolesManager.Verify(m_ => m_.FindByNameAsync(newRoleName), Times.Once);
        }

        [Fact]
        public async Task CheckAndSaveNewRoleAsync_NameTaken()
        {
            // Arrange
            var newRoleName = "New Role 1";
            var mockAspNetRolesManager = new Mock<IAspNetRolesManager>();
            mockAspNetRolesManager.Setup(m_ => m_.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole<string>());

            // Act
            var errorMessage = await CreateRole.CheckAndSaveNewRoleAsync(mockAspNetRolesManager.Object, new SaveNewRoleViewModel {RoleName = newRoleName});

            // Assert
            errorMessage.Should().NotBeNull();
            mockAspNetRolesManager.Verify(m_ => m_.FindByNameAsync(newRoleName), Times.Once);
        }
    }
}