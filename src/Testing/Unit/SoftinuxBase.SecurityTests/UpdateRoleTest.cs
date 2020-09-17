// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
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
    public class UpdateRoleTest
    {
        #region CheckAndUpdateRoleAsync
        [Fact]
        public async Task CheckAndUpdateRoleAsync_Ok_NoRoleByThisName()
        {
            // Arrange
            var roleManager = new Mock<IAspNetRolesManager>();
            var roleName = Guid.NewGuid().ToString();
            var roleId = Guid.NewGuid().ToString();
            var role = new IdentityRole<string> { Id = roleId, Name = Guid.NewGuid().ToString() };
            var model = new UpdateRoleViewModel { RoleId = roleId, RoleName = roleName };
            roleManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(default(IdentityRole<string>));
            roleManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(role);

            // Act
            var result = await UpdateRole.CheckAndUpdateRoleAsync(roleManager.Object, model);

            // Assert
            result.Should().BeNull();
            roleManager.Verify(m => m.FindByNameAsync(roleName), Times.Once);
            roleManager.Verify(m => m.FindByIdAsync(roleId), Times.Once);
            roleManager.Verify(m => m.SetRoleNameAsync(It.Is<IdentityRole<string>>(r => r.Id == roleId), roleName), Times.Once);
        }

        [Fact]
        public async Task CheckAndUpdateRoleAsync_Ok_RoleByThisNameSameId()
        {
            // Arrange
            var roleManager = new Mock<IAspNetRolesManager>();
            var roleName = Guid.NewGuid().ToString();
            var roleId = Guid.NewGuid().ToString();
            var role = new IdentityRole<string> { Id = roleId, Name = roleName };
            var model = new UpdateRoleViewModel { RoleId = roleId, RoleName = roleName };
            roleManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);
            roleManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(role);

            // Act
            var result = await UpdateRole.CheckAndUpdateRoleAsync(roleManager.Object, model);

            // Assert
            result.Should().BeNull();
            roleManager.Verify(m => m.FindByNameAsync(roleName), Times.Once);
            roleManager.Verify(m => m.FindByIdAsync(roleId), Times.Once);
            roleManager.Verify(m => m.SetRoleNameAsync(It.Is<IdentityRole<string>>(r => r.Id == roleId), roleName), Times.Once);
        }

        [Fact]
        public async Task CheckAndUpdateRoleAsync_Nok_RoleByThisNameOtherId()
        {
            // Arrange
            var roleManager = new Mock<IAspNetRolesManager>();
            var roleName = Guid.NewGuid().ToString();
            var roleId = Guid.NewGuid().ToString();
            var otherRoleId = Guid.NewGuid().ToString();
            var role = new IdentityRole<string> { Id = otherRoleId, Name = roleName };
            var model = new UpdateRoleViewModel { RoleId = roleId, RoleName = roleName };
            roleManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);

            // Act
            var result = await UpdateRole.CheckAndUpdateRoleAsync(roleManager.Object, model);

            // Assert
            result.Should().NotBeNull();
            roleManager.Verify(m => m.FindByNameAsync(roleName), Times.Once);
            roleManager.Verify(m => m.FindByIdAsync(It.IsAny<string>()), Times.Never);
            roleManager.Verify(m => m.SetRoleNameAsync(It.IsAny<IdentityRole<string>>(), It.IsAny<string>()), Times.Never);
        }

        #endregion

    }
}