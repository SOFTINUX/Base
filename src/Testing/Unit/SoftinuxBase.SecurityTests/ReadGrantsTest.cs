// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel;
using System.Linq;
using ExtCore.Data.Abstractions;
using FluentAssertions;
using Moq;
using SampleExtension1;
using SampleExtension2;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Permissions.Enums;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.SecurityTests.Mocks;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.SecurityTests
{
    public class ReadGrantsTest
    {
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
            model.RolesWithPermissions.Keys.Should().Contain(Constants.SampleExtension1AssemblyShortName);
            model.RolesWithPermissions.Keys.Should().Contain(Constants.SampleExtension2AssemblyShortName);
            model.RolesWithPermissions.Keys.Should().NotContain(Constants.SoftinuxBaseTestsCommonAssemblyShortName);
            model.RolesWithPermissions.Keys.Should().NotContain(Constants.SampleExtension3AssemblyShortName);

            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].Should().HaveCount(24);
            model.RolesWithPermissions[Constants.SampleExtension1AssemblyShortName].Should().HaveCount(3);
            model.RolesWithPermissions[Constants.SampleExtension2AssemblyShortName].Should().HaveCount(3);

            // Detailed assertions about SoftinuxBaseSecurityAssemblyShortName's permissions/roles
            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)Permissions.CreateRoles).Value.Should().HaveCount(1);
            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)Permissions.CreateRoles).Value.Should().Contain(Roles.Administrator.ToString());

            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)Permissions.DeleteRoles).Value.Should().HaveCount(1);
            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)Permissions.DeleteRoles).Value.Should().Contain(Roles.Administrator.ToString());

            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)Permissions.ListRoles).Value.Should().HaveCount(2);
            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)Permissions.ListRoles).Value.Should().Contain(Roles.Moderator.ToString());
            model.RolesWithPermissions[Constants.SoftinuxBaseSecurityAssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)Permissions.ListRoles).Value.Should().Contain(Roles.Administrator.ToString());

            // Detailed assertions about SampleExtension1AssemblyShortName's permissions/roles
            model.RolesWithPermissions[Constants.SampleExtension1AssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)SamplePermissions.Admin).Value.Should().HaveCount(1);
            model.RolesWithPermissions[Constants.SampleExtension1AssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)SamplePermissions.Admin).Value.Should().Contain(Roles.Administrator.ToString());

            model.RolesWithPermissions[Constants.SampleExtension1AssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)SamplePermissions.Write).Value.Should().HaveCount(1);
            model.RolesWithPermissions[Constants.SampleExtension1AssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)SamplePermissions.Write).Value.Should().Contain(Roles.Moderator.ToString());

            model.RolesWithPermissions[Constants.SampleExtension1AssemblyShortName].FirstOrDefault(kv => kv.Key.PermissionEnumValue == (short)SamplePermissions.Other).Value.Should().BeNull();

            // Detailed assertions about SampleExtension2AssemblyShortName's permissions/roles
            model.RolesWithPermissions[Constants.SampleExtension2AssemblyShortName].First(kv => kv.Key.PermissionEnumValue == (short)SamplePermissions2.Admin).Value.Should().HaveCount(0);

            model.RoleNames.Should().HaveCount(2);
            model.RoleNames.Should().Contain(Roles.Administrator.ToString());
            model.RoleNames.Should().Contain(Roles.Moderator.ToString());
        }
    }
}