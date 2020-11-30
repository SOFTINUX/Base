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
using SoftinuxBase.Security.Permissions.Enums;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.SecurityTests.Mocks;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.SecurityTests
{
    public class UpdateRoleAndGrantsTest
    {
        #region update role's permissions
        /// <summary>
        /// Test with incorrect extension name.
        /// </summary>
        [Fact]
        public async Task UpdateRoleToPermissionsAsync_Add_ExtensionNotFound()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();
            var storageMock = new Mock<IStorage>();
            var roleManager = new Mock<IAspNetRolesManager>();

            var roleName = "RoleXXX";
            var extensionName = "ExtensionXXX";
            short permissionValue = 1;

            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(storageMock.Object, roleManager.Object, roleName, extensionName, permissionValue, true);

            // Assert
            result.Should().Be($"Extension {extensionName} does not exist");

            roleToPermissionsRepositoryMock.Verify(m_ => m_.FindBy(It.IsAny<string>()), Times.Never);
            roleToPermissionsRepositoryMock.Verify(m_ => m_.SetPermissions(It.IsAny<string>(), It.IsAny<PermissionsDictionary>()), Times.Never);
            storageMock.Verify(m_ => m_.SaveAsync(), Times.Never);
        }

        /// <summary>
        /// Test with incorrect extension because it doesn't define a Type to hold its permission enum.
        /// </summary>
        [Fact]
        public async Task UpdateRoleToPermissionsAsync_Add_ExtensionWithoutPermission()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();
            var roleManager = new Mock<IAspNetRolesManager>();

            var roleName = "RoleXXX";
            var extensionName = Constants.SampleExtension3AssemblyShortName;
            short permissionValue = 1;

            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(storageMock.Object, roleManager.Object, roleName, extensionName, permissionValue, true);

            // Assert
            result.Should().Be($"Extension {extensionName} doesn't define a Type to hold permissions");

            roleToPermissionsRepositoryMock.Verify(m_ => m_.FindBy(It.IsAny<string>()), Times.Never);
            roleToPermissionsRepositoryMock.Verify(m_ => m_.SetPermissions(It.IsAny<string>(), It.IsAny<PermissionsDictionary>()), Times.Never);
            storageMock.Verify(m_ => m_.SaveAsync(), Times.Never);
        }

        /// <summary>
        /// Test with incorrect extension because the role doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateRoleToPermissionsAsync_Add_RoleNotFound()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();
            var roleManager = new Mock<IAspNetRolesManager>();

            var roleName = "RoleXXX";
            var extensionName = Constants.SoftinuxBaseSecurityAssemblyShortName;
            short permissionValue = 1;

            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(storageMock.Object, roleManager.Object, roleName, extensionName, permissionValue, true);

            // Assert
            result.Should().Be($"Role {roleName} does not exist");

            roleToPermissionsRepositoryMock.Verify(m_ => m_.FindBy(It.IsAny<string>()), Times.Never);
            roleToPermissionsRepositoryMock.Verify(m_ => m_.SetPermissions(It.IsAny<string>(), It.IsAny<PermissionsDictionary>()), Times.Never);
            storageMock.Verify(m_ => m_.SaveAsync(), Times.Never);
        }

        /// <summary>
        /// Test that should pass.
        /// </summary>
        [Fact]
        public async Task UpdateRoleToPermissionsAsync_Add_Ok()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();
            var roleManager = new Mock<IAspNetRolesManager>();

            var roleName = Roles.Administrator.ToString();
            var extensionName = Constants.SoftinuxBaseSecurityAssemblyShortName;
            short permissionValue = (short)Permissions.Read;

            roleManager.Setup(m_ => m_.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole<string>(roleName));
            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(storageMock.Object, roleManager.Object, roleName, extensionName, permissionValue, true);

            // Assert
            result.Should().Be(null);

            roleManager.Verify(m_ => m_.FindByNameAsync(roleName), Times.Once);
            roleToPermissionsRepositoryMock.Verify(m_ => m_.FindBy(roleName), Times.Once);
            roleToPermissionsRepositoryMock.Verify(m_ => m_.SetPermissions(roleName, It.IsAny<PermissionsDictionary>()), Times.Once);
            storageMock.Verify(m_ => m_.SaveAsync(), Times.Once);
        }

        /// <summary>
        /// Test that should abort.
        /// </summary>
        [Fact]
        public async Task UpdateRoleToPermissionsAsync_Remove_RoleHasNoPermissions()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();
            var roleManager = new Mock<IAspNetRolesManager>();

            var roleName = Roles.Administrator.ToString();
            var extensionName = Constants.SoftinuxBaseSecurityAssemblyShortName;
            short permissionValue = (short)Permissions.Read;

            roleManager.Setup(m_ => m_.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole<string>(roleName));
            roleToPermissionsRepositoryMock.Setup(m_ => m_.FindBy(roleName)).Returns(default(RoleToPermissions));
            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(storageMock.Object, roleManager.Object, roleName, extensionName, permissionValue, false);

            // Assert
            result.Should().Be($"Cannot remove permission for role {roleName} that has no permissions");

            roleManager.Verify(m_ => m_.FindByNameAsync(roleName), Times.Once);
            roleToPermissionsRepositoryMock.Verify(m_ => m_.FindBy(roleName), Times.Once);
            roleToPermissionsRepositoryMock.Verify(m_ => m_.SetPermissions(It.IsAny<string>(), It.IsAny<PermissionsDictionary>()), Times.Never);
            storageMock.Verify(m_ => m_.SaveAsync(), Times.Never);
        }

        #endregion

        #region update permissions on role's name change

        /// <summary>
        /// A renamed role has some permissions.
        /// </summary>
        [Fact]
        public async Task UpdateRoleToPermissionsAsync_RenameRole_HasPermissions()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();

            var roleName = Roles.Administrator.ToString();
            var newRoleName = "testAdmin";
            var existingRecord = new RoleToPermissions(roleName, roleName, new PermissionsDictionary());

            roleToPermissionsRepositoryMock.Setup(m_ => m_.FindBy(roleName)).Returns(existingRecord);
            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(storageMock.Object, roleName, newRoleName);

            // Assert
            result.Should().Be(true);

            roleToPermissionsRepositoryMock.Verify(m_ => m_.FindBy(roleName), Times.Once);
            storageMock.Verify(m_ => m_.SaveAsync(), Times.Once);
        }

        /// <summary>
        /// A renamed role has no permissions.
        /// </summary>
        [Fact]
        public async Task UpdateRoleToPermissionsAsync_RenameRole_HasNoPermissions()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();

            var roleName = Roles.Administrator.ToString();
            var newRoleName = "testAdmin";

            roleToPermissionsRepositoryMock.Setup(m_ => m_.FindBy(roleName)).Returns(default(RoleToPermissions));
            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(storageMock.Object, roleName, newRoleName);

            // Assert
            result.Should().Be(null);

            roleToPermissionsRepositoryMock.Verify(m_ => m_.FindBy(roleName), Times.Once);
            storageMock.Verify(m_ => m_.SaveAsync(), Times.Never);
        }
        #endregion
    }
}