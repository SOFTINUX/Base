// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Enums;
using Security.Administration;
using Security.Data.Abstractions;
using SecurityTest.Util;
using Xunit;
using Xunit.Abstractions;

namespace SecurityTest
{
    /// <summary>
    /// Test that calling FillDatabase on a database with already recorded data doesn't create duplicate records.
    /// Test with the data related to Security extension.
    /// Uses the standard filled database of unit tests.
    /// </summary>
    [TestCaseOrderer("SecurityTest.PriorityOrderer", "SecurityTest")]
    [Collection("Database collection")]
    public class FillDatabaseOnNonEmptyDbTest : BaseTest
    {
        private const string _securityAssemblyName = "Security";
        public FillDatabaseOnNonEmptyDbTest(DatabaseFixture fixture_, ITestOutputHelper outputHandler_)
        {
            _fixture = fixture_;
            _outputHandler = outputHandler_;

        }

        [Fact, TestPriority(0)]
        public void Test()
        {
            List<KeyValuePair<int, string>> initialCounters = CountSecurityExtensionRecords();

            IServiceProvider testProvider = new MockedServiceProvider(_fixture);
            new FillDatabase().Execute(null, testProvider);

            List<KeyValuePair<int, string>> counterAfterFilling = CountSecurityExtensionRecords();

            VerifyThatWeHaveTheSameCounters(initialCounters, counterAfterFilling);

        }

        private void VerifyThatWeHaveTheSameCounters(List<KeyValuePair<int, string>> initialCounters_, List<KeyValuePair<int, string>> finalCounters_)
        {
            for (int i = 0; i < initialCounters_.Count(); i++)
            {
                _outputHandler.WriteLine("Comparing counters for " + initialCounters_[i].Value);
                Assert.Equal(initialCounters_[i].Key, finalCounters_[i].Key);
            }
        }

        /// <summary>
        /// Query how many permissions, roles etc we have for Security extension and generates a list of counters with textual description.
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> CountSecurityExtensionRecords()
        {
            List<KeyValuePair<int, string>> counters = new List<KeyValuePair<int, string>>();

            // 0. Permissions
            counters.Add(
                new KeyValuePair<int, string>(_fixture.GetRepository<IPermissionRepository>().All().Count(e_ => e_.OriginExtension == _securityAssemblyName),
                "Number of permissions for Security extension"));
            // 1. Roles
            counters.Add(
                new KeyValuePair<int, string>(_fixture.GetRepository<IRoleRepository>().All().Count(e_ => e_.OriginExtension == _securityAssemblyName),
                    "Number of roles for Security extension"));
            // 2. Users
            counters.Add(
                new KeyValuePair<int, string>(_fixture.GetRepository<IUserRepository>().All().Count(e_ => e_.OriginExtension == _securityAssemblyName),
                    "Number of users for Security extension"));
            // 3. CredentialType
            counters.Add(
                new KeyValuePair<int, string>(_fixture.GetRepository<ICredentialTypeRepository>().All().Count(e_ => e_.OriginExtension == _securityAssemblyName),
                    "Number of credential types for Security extension"));
            // No groups inserted by this extension

            // 4. Role-permission links: all permissions linked to administrator-owner role
            int roleId = _fixture.GetRepository<IRoleRepository>()
                .FindBy(Roles.ROLE_CODE_ADMINISTRATOR_OWNER, _securityAssemblyName).Id;
            counters.Add(
                new KeyValuePair<int, string>(_fixture.GetRepository<IRolePermissionRepository>().FilteredByRoleId(roleId).Count(),
                "Number of role-permission links for Security extension"));

            return counters;
        }
    }
}
