// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.SecurityTests.Mocks;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.SecurityTests
{
    /// <summary>
    /// Test the deletion of permission grants, from test extensions and mocked database role-to-permission data.
    /// </summary>
    public class DeleteRoleTest
    {
        [Fact]
        public async Task DeleteRolePermissionsAsync_RoleNotFound()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();
            var aspNetRolesManagerMock = new Mock<IAspNetRolesManager>();
            var storageMock = new Mock<IStorage>();

            var roleName = "Role1";

            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);
            aspNetRolesManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(default(IdentityRole<string>));

            // Act
            var operationResult = await DeleteRole.DeleteRolePermissionsAsync(aspNetRolesManagerMock.Object, storageMock.Object, roleName);

            // Assert
            operationResult.Should().BeNull();

            aspNetRolesManagerMock.Verify(m => m.FindByNameAsync(roleName), Times.Once);
            storageMock.Verify(s => s.GetRepository<IRoleToPermissionsRepository>(), Times.Never);
            roleToPermissionsRepositoryMock.Verify(r => r.FindBy(It.IsAny<string>()), Times.Never);
            roleToPermissionsRepositoryMock.Verify(r => r.Delete(It.IsAny<RoleToPermissions>()), Times.Never);
        }

        [Fact]
        public async Task DeleteRolePermissionsAsync_RoleWithoutPermissions()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();
            var aspNetRolesManagerMock = new Mock<IAspNetRolesManager>();
            var storageMock = new Mock<IStorage>();

            var roleName = "Role1";

            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);
            aspNetRolesManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole<string> { Name = roleName });

            // Act
            var operationResult = await DeleteRole.DeleteRolePermissionsAsync(aspNetRolesManagerMock.Object, storageMock.Object, roleName);

            // Assert
            operationResult.Should().BeNull();

            aspNetRolesManagerMock.Verify(m => m.FindByNameAsync(roleName), Times.Once);
            storageMock.Verify(s => s.GetRepository<IRoleToPermissionsRepository>(), Times.Once);
            roleToPermissionsRepositoryMock.Verify(r => r.FindBy(roleName), Times.Once);
            roleToPermissionsRepositoryMock.Verify(r => r.Delete(It.IsAny<RoleToPermissions>()), Times.Never);
        }

        [Fact]
        public async Task DeleteRolePermissionsAsync_Ok()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();
            var aspNetRolesManagerMock = new Mock<IAspNetRolesManager>();
            var storageMock = new Mock<IStorage>();

            var roleName = Roles.Administrator.ToString();
            var roleToPermissionsEntity = new RoleToPermissions(roleName, null, new PermissionsDictionary());

            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);
            aspNetRolesManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole<string> {Name = roleName});
            roleToPermissionsRepositoryMock.Setup(r => r.FindBy(It.IsAny<string>())).Returns(roleToPermissionsEntity);

            // Act
            var operationResult = await DeleteRole.DeleteRolePermissionsAsync(aspNetRolesManagerMock.Object, storageMock.Object, roleName);

            // Assert
            operationResult.Should().Be(true);

            aspNetRolesManagerMock.Verify(m => m.FindByNameAsync(roleName), Times.Once);
            storageMock.Verify(s => s.GetRepository<IRoleToPermissionsRepository>(), Times.Once);
            roleToPermissionsRepositoryMock.Verify(r => r.FindBy(roleName), Times.Once);
            roleToPermissionsRepositoryMock.Verify(r => r.Delete(roleToPermissionsEntity), Times.Once);
        }
    }
}