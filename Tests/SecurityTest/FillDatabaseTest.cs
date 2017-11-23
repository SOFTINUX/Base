// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.ServiceConfiguration;
using SecurityTest.Util;
using Xunit;

namespace SecurityTest
{
    /// <summary>
    /// Unlike the other tests, this one doesn't use collection fixture DatabaseFixture (by using DatabaseCollection attribute)
    /// but uses a class fixture EmptyDatabaseFixture.
    /// </summary>
    [TestCaseOrderer("SecurityTest.PriorityOrderer", "SecurityTest")]
    public class FillDatabaseTest : BaseTest, IClassFixture<EmptyDatabaseFixture>
    {
        public FillDatabaseTest(EmptyDatabaseFixture fixture_)
        {
            _fixture = fixture_;

        }

        [Fact, TestPriority(0)]
        public void Test()
        {
            Assert.Empty(_fixture.GetRepository<IPermissionRepository>().All());
            IServiceProvider testProvider = new MockedServiceProvider(_fixture);
            new FillDatabase().Execute(null, testProvider);
            IEnumerable<Permission> perms = _fixture.GetRepository<IPermissionRepository>().All();
            Assert.NotEmpty(perms);
            foreach (Permission perm in perms)
                Console.WriteLine("[" + perm.Id + "] " + perm.Code);
        }
    }
}
