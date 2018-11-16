// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using CommonTest;
using SoftinuxBase.Security.Data.Abstractions;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class BasicReadingTest : CommonTestWithDatabase
    {
        public BasicReadingTest(DatabaseFixture databaseFixture_) : base(databaseFixture_)
        {

        }

        /// <summary>
        /// Test that we are able to read data using Storage and ExtCore repository pattern.
        /// </summary>
        [Fact]
        public void TestReadWithRepository()
        {
            var data = DatabaseFixture.Storage.GetRepository<IPermissionRepository>().All();
            Assert.NotNull(data);
        }

        /// <summary>
        /// Test that we are able to read data using Identity.
        /// </summary>
        [Fact]
        public void TestReadWithIdentity()
        {
            var data = DatabaseFixture.RoleManager.Roles.ToList();
            Assert.NotNull(data);
        }
    }
}