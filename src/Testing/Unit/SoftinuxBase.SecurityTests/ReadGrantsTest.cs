// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using FluentAssertions;
using Moq;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.SecurityTests.Mocks;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.SecurityTests
{
    [Collection("Database collection")]
    public class ReadGrantsTest : CommonTestWithDatabase
    {
        public ReadGrantsTest(DatabaseFixture databaseFixture_) : base(databaseFixture_)
        {
        }

        /// <summary>
        /// Uses a mock for database data.
        /// </summary>
        [Fact]
        [Category("Mock")]
        public void ReadAll()
        {
            // Arrange
            Fakes.ExtensionManager.Setup();
            var roleToPermissionsRepositoryMock = new RoleToPermissionsRepositoryMock();

            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s_ => s_.GetRepository<IRoleToPermissionsRepository>()).Returns(roleToPermissionsRepositoryMock.Object);

            // Act
            var model = ReadGrants.ReadAll(storageMock.Object);

            // Assert
            model.Should().NotBeNull();
            model.PermissionsByRoleAndExtension.Keys.Should().NotBeEmpty();
            model.PermissionsByRoleAndExtension.Keys.Should().Contain(Constants.SoftinuxBaseSecurityAssemblyShortName);
            model.PermissionsByRoleAndExtension.Keys.Should().Contain("SampleExtension1");
            model.PermissionsByRoleAndExtension.Keys.Should().NotContain(Constants.SoftinuxBaseTestsCommonAssemblyShortName);

            model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurityAssemblyShortName][Roles.Administrator.ToString()].Should().HaveCount(2);
            model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurityAssemblyShortName][Roles.Moderator.ToString()].Should().HaveCount(2);
            model.PermissionsByRoleAndExtension["SampleExtension1"][Roles.Administrator.ToString()].Should().HaveCount(1);
            model.PermissionsByRoleAndExtension["SampleExtension1"][Roles.Moderator.ToString()].Should().HaveCount(2);

            model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurityAssemblyShortName][Roles.Administrator.ToString()].FirstOrDefault(permissionDisplay_ => permissionDisplay_.GroupName == "Roles" && permissionDisplay_.ShortName == "CanCreate").Should().NotBeNull();
            model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurityAssemblyShortName][Roles.Administrator.ToString()].FirstOrDefault(permissionDisplay_ => permissionDisplay_.GroupName == "Roles" && permissionDisplay_.ShortName == "CanCreate").Should().NotBeNull();
            model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurityAssemblyShortName][Roles.Moderator.ToString()].FirstOrDefault(permissionDisplay_ => permissionDisplay_.GroupName == "Roles" && permissionDisplay_.ShortName == "CanList").Should().NotBeNull();
            model.PermissionsByRoleAndExtension[Constants.SoftinuxBaseSecurityAssemblyShortName][Roles.Moderator.ToString()].FirstOrDefault(permissionDisplay_ => permissionDisplay_.GroupName == "Roles" && permissionDisplay_.ShortName == "CanRead").Should().NotBeNull();
            model.PermissionsByRoleAndExtension["SampleExtension1"][Roles.Moderator.ToString()].FirstOrDefault(permissionDisplay_ => permissionDisplay_.GroupName == "Sample" && permissionDisplay_.ShortName == "Write").Should().NotBeNull();
            model.PermissionsByRoleAndExtension["SampleExtension1"][Roles.Moderator.ToString()].FirstOrDefault(permissionDisplay_ => permissionDisplay_.GroupName == "Sample" && permissionDisplay_.ShortName == "Other").Should().BeNull();
        }

        /// <summary>
        /// Uses the database to test the query to table.
        /// </summary>
        [Fact]
        [Category("Database")]
        public void ReadAll_QueryDatabase()
        {
            // Arrange

            // Act
            var model = ReadGrants.ReadAll(DatabaseFixture.Storage);

            // Assert
            model.Should().NotBeNull();
            model.PermissionsByRoleAndExtension.Keys.Should().NotBeEmpty();
        }
    }
}