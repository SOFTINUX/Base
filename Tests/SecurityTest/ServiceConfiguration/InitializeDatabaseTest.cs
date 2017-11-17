// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Security;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.ServiceConfiguration;
using SecurityTest.Util;
using Xunit;

namespace SecurityTest.ServiceConfiguration
{
    [Collection("Database collection")]
    public class InitializeDatabaseTest : BaseTest
    {
        public InitializeDatabaseTest(DatabaseFixture fixture_)
        {
            _fixture = fixture_;

            // initialize test file from copy. Root dir is bin/debug/netcoreapp2.0
            File.Copy("../../../../Artefacts/basedb_empty.sqlite", "../../../../WorkDir/basedb_tests.sqlite", true);
        }

        [Fact]
        public void Test()
        {
            Assert.Empty(_fixture.GetRepository<IPermissionRepository>().All());
            System.IServiceProvider testProvider = new MockedServiceProvider(_fixture);
            new InitializeDatabase().Execute(null, testProvider);
            IEnumerable<Permission> perms = _fixture.GetRepository<IPermissionRepository>().All();
            Assert.NotEmpty(perms);
            foreach (Permission perm in perms)
                Console.WriteLine("[" + perm.Id + "] " + perm.Code);
        }
    }
}