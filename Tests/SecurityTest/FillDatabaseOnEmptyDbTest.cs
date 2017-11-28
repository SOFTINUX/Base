// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.ServiceConfiguration;
using SecurityTest.Util;
using Xunit;
using Xunit.Abstractions;

namespace SecurityTest
{
    /// <summary>
    /// Unlike the other tests, this one doesn't use collection fixture DatabaseFixture (by using DatabaseCollection attribute)
    /// but uses a class fixture EmptyDatabaseFixture.
    /// </summary>
    [TestCaseOrderer("SecurityTest.PriorityOrderer", "SecurityTest")]
    public class FillDatabaseOnEmptyDbTest : BaseTest, IClassFixture<EmptyDatabaseFixture>
    {
        public FillDatabaseOnEmptyDbTest(EmptyDatabaseFixture fixture_, ITestOutputHelper outputHandler_)
        {
            _fixture = fixture_;
            _outputHandler = outputHandler_;

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
                _outputHandler.WriteLine("[" + perm.Id + "] " + perm.Code);
        }
    }
}
