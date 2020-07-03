// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using FluentAssertions;
using Moq;
using SampleExtension1;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Permissions.Enums;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.SecurityTests.Mocks;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.SecurityTests
{
    [Collection("Database collection")]
    public class UpdateRoleAndGrantsTest : CommonTestWithDatabase
    {
        public UpdateRoleAndGrantsTest(DatabaseFixture databaseFixture_) : base(databaseFixture_)
        {
        }

        /// <summary>
        /// Test with incorrect extension name.
        /// Uses a mock for database data.
        /// </summary>
        [Fact]
        [Category("Mock")]
        public void UpdateRoleToPermissionsAsync_Add_ExtensionNotFound()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);
            var roleManager = DatabaseFixture.RoleManager;
            var roleName = "RoleXXX";
            var extensionName = "ExtensionXXX";
            short permissionValue = 1;

            // Act
            var result = UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(roleManager, storageMock.Object, roleName, extensionName, permissionValue, true);

            // Assert
            result.Should().Be($"Extension {extensionName} does not exist");
            roleToPermissionsRepositoryMock.Verify(m => m.FindBy(It.IsAny<string>()), Times.Never);
            storageMock.Verify(m => m.SaveAsync(), Times.Never);
        }

        /// <summary>
        /// Test with incorrect extension because it doesn't define a Type to hold its permission enum.
        /// Uses a mock for database data.
        /// </summary>
        [Fact]
        [Category("Mock")]
        public async Task UpdateRoleToPermissionsAsync_Add_ExtensionWithoutPermission()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);
            var roleManager = DatabaseFixture.RoleManager;
            var roleName = "RoleXXX";
            var extensionName = Constants.SampleExtension3AssemblyShortName;
            short permissionValue = 1;

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(roleManager, storageMock.Object, roleName, extensionName, permissionValue, true);

            // Assert
            result.Should().Be($"Extension {extensionName} does not exist");
            roleToPermissionsRepositoryMock.Verify(m => m.FindBy(It.IsAny<string>()), Times.Never);
            storageMock.Verify(m => m.SaveAsync(), Times.Never);
        }

        /// <summary>
        /// Test with incorrect extension because the role doesn't exist.
        /// Uses a mock for database data.
        /// </summary>
        [Fact]
        [Category("Mock")]
        public async Task UpdateRoleToPermissionsAsync_Add_RoleNotFound()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);
            var roleManager = DatabaseFixture.RoleManager;
            var roleName = "RoleXXX";
            var extensionName = Constants.SoftinuxBaseSecurityAssemblyShortName;
            short permissionValue = 1;

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(roleManager, storageMock.Object, roleName, extensionName, permissionValue, true);

            // Assert
            result.Should().Be($"Role {roleName} does not exist");
            roleToPermissionsRepositoryMock.Verify(m => m.FindBy(It.IsAny<string>()), Times.Never);
            storageMock.Verify(m => m.SaveAsync(), Times.Never);
        }

        /// <summary>
        /// Test that should pass.
        /// Uses a mock for database data.
        /// </summary>
        [Fact]
        [Category("Mock")]
        public async Task UpdateRoleToPermissionsAsync_Add_Ok()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);
            var roleManager = DatabaseFixture.RoleManager;
            var roleName = Roles.Administrator.ToString();
            var extensionName = Constants.SoftinuxBaseSecurityAssemblyShortName;
            short permissionValue = (short)Permissions.Read;

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(roleManager, storageMock.Object, roleName, extensionName, permissionValue, true);

            // Assert
            result.Should().Be(null);
            roleToPermissionsRepositoryMock.Verify(m => m.FindBy(roleName), Times.Once);
            storageMock.Verify(m => m.SaveAsync(), Times.Once);
        }

        /// <summary>
        /// Test that should abort.
        /// Uses a mock for database data.
        /// </summary>
        [Fact]
        [Category("Mock")]
        public async Task UpdateRoleToPermissionsAsync_Remove_RoleHasNoPermissions()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);
            var roleManager = DatabaseFixture.RoleManager;
            var roleName = Roles.Administrator.ToString();
            roleToPermissionsRepositoryMock.Setup(m => m.FindBy(roleName)).Returns(default(RoleToPermissions));
            var extensionName = Constants.SoftinuxBaseSecurityAssemblyShortName;
            short permissionValue = (short)Permissions.Read;

            // Act
            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(roleManager, storageMock.Object, roleName, extensionName, permissionValue, false);

            // Assert
            result.Should().Be($"Cannot remove permission for role {roleName} that has no permissions");
            roleToPermissionsRepositoryMock.Verify(m => m.FindBy(roleName), Times.Once);
            storageMock.Verify(m => m.SaveAsync(), Times.Never);
        }
    }
}