// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.Security.CommonTests
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
            permissionsDictionary.Add(typeof(Common.Enums.Permissions), (short)Common.Enums.Permissions.CreateRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.Common").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.Common"].Should().BeEquivalentTo(new HashSet<short> { (short)Common.Enums.Permissions.CreateRoles });
        }

        [Fact]
        public void Add_OnePermissionTwice()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Common.Enums.Permissions), (short)Common.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Common.Enums.Permissions), (short)Common.Enums.Permissions.CreateRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.Common").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.Common"].Should().BeEquivalentTo(new HashSet<short> { (short)Common.Enums.Permissions.CreateRoles });
        }

        [Fact]
        public void Add_TwoPermissions()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Common.Enums.Permissions), (short)Common.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(Common.Enums.Permissions), (short)Common.Enums.Permissions.EditRoles);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.Common").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.Common"].Should().BeEquivalentTo(new HashSet<short> { (short)Common.Enums.Permissions.CreateRoles, (short)Common.Enums.Permissions.EditRoles });
        }

        [Fact]
        public void Add_PermissionsFromSeveralEnums()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.Add(typeof(Common.Enums.Permissions), (short)Common.Enums.Permissions.CreateRoles);
            permissionsDictionary.Add(typeof(OtherPermissions), (short)OtherPermissions.Read);

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(2);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.Common").Should().BeTrue();
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Tests.Common").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.Common"].Should().BeEquivalentTo(new HashSet<short> { (short)Common.Enums.Permissions.CreateRoles });
            permissionsDictionary.Dictionary["SoftinuxBase.Tests.Common"].Should().BeEquivalentTo(new HashSet<short> { (short)OtherPermissions.Read });
        }

        #endregion

        #region AddGrouped
        [Fact]
        public void AddGrouped()
        {
            // Arrange
            var permissionsDictionary = new PermissionsDictionary();

            // Act
            permissionsDictionary.AddGrouped(typeof(Common.Enums.Permissions).GetAssemblyShortName(), new List<short>{(short)Common.Enums.Permissions.CreateRoles, (short)Common.Enums.Permissions.DeleteRoles});

            // Assert
            permissionsDictionary.Dictionary.Keys.Count.Should().Be(1);
            permissionsDictionary.Dictionary.ContainsKey("SoftinuxBase.Security.Common").Should().BeTrue();
            permissionsDictionary.Dictionary["SoftinuxBase.Security.Common"].Should().BeEquivalentTo(new HashSet<short> { (short)Common.Enums.Permissions.CreateRoles, (short)Common.Enums.Permissions.DeleteRoles });
        }

        #endregion

        #region Merge
        [Fact]
        public void Merge()
        {
            // Arrange
            var permissionsDictionary1 = new PermissionsDictionary();
            permissionsDictionary1.AddGrouped(typeof(Common.Enums.Permissions).GetAssemblyShortName(), new List<short> { (short)Common.Enums.Permissions.CreateRoles, (short)Common.Enums.Permissions.DeleteRoles });

            var permissionsDictionary2 = new PermissionsDictionary();
            permissionsDictionary2.AddGrouped(typeof(Common.Enums.Permissions).GetAssemblyShortName(), new List<short> { (short)Common.Enums.Permissions.CreateRoles, (short)Common.Enums.Permissions.CreateUsers });

            var permissionsDictionary3 = new PermissionsDictionary();
            permissionsDictionary3.AddGrouped(typeof(OtherPermissions).GetAssemblyShortName(), new List<short> { (short)OtherPermissions.Write, (short)OtherPermissions.Read });

            // Act
            var merged = PermissionsDictionary.Merge(permissionsDictionary1, permissionsDictionary2, permissionsDictionary3);

            // Assert
            merged.Dictionary.Keys.Count.Should().Be(2);
            merged.Dictionary.ContainsKey("SoftinuxBase.Security.Common").Should().BeTrue();
            merged.Dictionary["SoftinuxBase.Security.Common"].Should().BeEquivalentTo(new HashSet<short> { (short)Common.Enums.Permissions.CreateRoles, (short)Common.Enums.Permissions.DeleteRoles, (short)Common.Enums.Permissions.CreateUsers });
            merged.Dictionary.ContainsKey("SoftinuxBase.Tests.Common").Should().BeTrue();
            merged.Dictionary["SoftinuxBase.Tests.Common"].Should().BeEquivalentTo(new HashSet<short> { (short)OtherPermissions.Read, (short)OtherPermissions.Write });

        }

        #endregion
    }
}
