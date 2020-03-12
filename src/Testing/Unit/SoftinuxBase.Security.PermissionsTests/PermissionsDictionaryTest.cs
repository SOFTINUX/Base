// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using SampleExtension1;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Tests.Common;
using Xunit;
using Constants = SoftinuxBase.Tests.Common.Constants;

namespace SoftinuxBase.Security.PermissionsTests
{
    public class PermissionsDictionaryTest
    {
        #region Add

        [Fact]
        public void Add_OnePermission()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey(Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName).Should().BeTrue();
            permissionsDictionary.Dictionary[Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName].Should().BeEquivalentTo(new HashSet<short> {(short)Permissions.Enums.Permissions.CreateRoles});
        }

        [Fact]
        public void Add_OnePermissionTwice()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey(Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName).Should().BeTrue();
            permissionsDictionary.Dictionary[Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName].Should().BeEquivalentTo(new HashSet<short> {(short)Permissions.Enums.Permissions.CreateRoles});
        }

        [Fact]
        public void Add_TwoPermissions()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.EditRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey(Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName).Should().BeTrue();
            permissionsDictionary.Dictionary[Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName].Should().BeEquivalentTo(new HashSet<short> {(short)Permissions.Enums.Permissions.CreateRoles, (short)Permissions.Enums.Permissions.EditRoles});
        }

        [Fact]
        public void Add_PermissionsFromSeveralEnums()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(OtherPermissions), (short)OtherPermissions.Read);
            permissionsDictionary.Add(typeof(SamplePermissions), (short)SamplePermissions.Write);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(3);
            permissionsDictionary.Dictionary.ContainsKey(Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName).Should().BeTrue();
            permissionsDictionary.Dictionary.ContainsKey(Constants.SoftinuxBaseTestsCommonOtherPermissionsEnumAssemblyQualifiedName).Should().BeTrue();
            permissionsDictionary.Dictionary[Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName].Should().BeEquivalentTo(new HashSet<short> {(short)Permissions.Enums.Permissions.CreateRoles});
            permissionsDictionary.Dictionary[Constants.SampleExtension1SamplePermissionsEnumAssemblyQualifiedName].Should().BeEquivalentTo(new HashSet<short> {(short)SamplePermissions.Write});
        }

        #endregion

        #region AddGrouped

        [Fact]
        public void AddGrouped()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.AddGrouped(typeof(Permissions.Enums.Permissions).AssemblyQualifiedName, new List<short> {(short)Permissions.Enums.Permissions.CreateRoles, (short)Permissions.Enums.Permissions.DeleteRoles});

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey(Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName).Should().BeTrue();
            permissionsDictionary.Dictionary[Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName].Should().BeEquivalentTo(new HashSet<short> {(short)Permissions.Enums.Permissions.CreateRoles, (short)Permissions.Enums.Permissions.DeleteRoles});
        }

        #endregion

        #region Merge

        [Fact]
        public void Merge()
        {
            // Arrange
            var permissionsDictionary1 = new PermissionsDictionary();
            permissionsDictionary1.AddGrouped(typeof(Permissions.Enums.Permissions).AssemblyQualifiedName, new List<short> {(short)Permissions.Enums.Permissions.CreateRoles, (short)Permissions.Enums.Permissions.DeleteRoles});

            var permissionsDictionary2 = new PermissionsDictionary();
            permissionsDictionary2.AddGrouped(typeof(Permissions.Enums.Permissions).AssemblyQualifiedName, new List<short> {(short)Permissions.Enums.Permissions.CreateRoles, (short)Permissions.Enums.Permissions.CreateUsers});

            var permissionsDictionary3 = new PermissionsDictionary();
            permissionsDictionary3.AddGrouped(typeof(OtherPermissions).AssemblyQualifiedName, new List<short> {(short)OtherPermissions.Write, (short)OtherPermissions.Read});

            // Act
            var merged = PermissionsDictionary.Merge(permissionsDictionary1, permissionsDictionary2, permissionsDictionary3);

            // Assert
            merged.Dictionary.Keys.Count.Should().Be(2);
            merged.Dictionary.ContainsKey(Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName).Should().BeTrue();
            merged.Dictionary[Constants.SoftinuxBaseSecurityPermissionsPermissionsEnumAssemblyQualifiedName].Should().BeEquivalentTo(new HashSet<short> {(short)Permissions.Enums.Permissions.CreateRoles, (short)Permissions.Enums.Permissions.DeleteRoles, (short)Permissions.Enums.Permissions.CreateUsers});
            merged.Dictionary.ContainsKey(Constants.SoftinuxBaseTestsCommonOtherPermissionsEnumAssemblyQualifiedName).Should().BeTrue();
            merged.Dictionary[Constants.SoftinuxBaseTestsCommonOtherPermissionsEnumAssemblyQualifiedName].Should().BeEquivalentTo(new HashSet<short> {(short)OtherPermissions.Read, (short)OtherPermissions.Write});
        }

        #endregion
    }
}