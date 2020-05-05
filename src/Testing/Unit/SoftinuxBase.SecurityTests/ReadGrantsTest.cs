// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel;
using System.Linq;
using ExtCore.Data.Abstractions;
using FluentAssertions;
using Moq;
using SampleExtension1;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Permissions.Enums;
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
            model.RolesWithPermissions.Keys.Should().NotBeEmpty();
            model.RolesWithPermissions.Keys.Should().Contain(Constants.SoftinuxBaseSecurityAssemblyShortName);
            model.RolesWithPermissions.Keys.Should().Contain("SampleExtension1");
            model.RolesWithPermissions.Keys.Should().Contain("SampleExtension2");
            model.RolesWithPermissions.Keys.Should().NotContain(Constants.SoftinuxBaseTestsCommonAssemblyShortName);

            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.Permission == (short)Permissions.CreateRoles).Value.Should().HaveCount(1);
            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.Permission == (short)Permissions.CreateRoles).Value.Should().Contain(Roles.Administrator.ToString());
            
            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.Permission == (short)Permissions.DeleteRoles).Value.Should().HaveCount(1);
            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.Permission == (short)Permissions.DeleteRoles).Value.Should().Contain(Roles.Administrator.ToString());

            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.Permission == (short)Permissions.ListRoles).Value.Should().HaveCount(2);
            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.Permission == (short)Permissions.ListRoles).Value.Should().Contain(Roles.Moderator.ToString());
            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.Permission == (short)Permissions.ListRoles).Value.Should().Contain(Roles.Administrator.ToString());

            model.RolesWithPermissions["SampleExtension1"].First(kv => kv.Key.Permission == (short)SamplePermissions.Admin).Value.Should().HaveCount(1);
            model.RolesWithPermissions["SampleExtension1"].First(kv => kv.Key.Permission == (short)SamplePermissions.Admin).Value.Should().Contain(Roles.Administrator.ToString());

            model.RolesWithPermissions["SampleExtension1"].First(kv => kv.Key.Permission == (short)SamplePermissions.Write).Value.Should().HaveCount(1);
            model.RolesWithPermissions["SampleExtension1"].First(kv => kv.Key.Permission == (short)SamplePermissions.Write).Value.Should().Contain(Roles.Moderator.ToString());

            model.RolesWithPermissions["SampleExtension1"].FirstOrDefault(kv => kv.Key.Permission == (short)SamplePermissions.Other).Value.Should().BeNull();
            
            model.RolesWithPermissions["SampleExtension2"].Should().HaveCount(0);
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
            model.RolesWithPermissions.Keys.Should().NotBeEmpty();
        }
    }
}