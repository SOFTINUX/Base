// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Security;
using Security.Data.Abstractions;
using SecurityTest.Util;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class DatabaseInitializerTest : BaseTest
    {
        public DatabaseInitializerTest(DatabaseFixture fixture_)
        {
            _fixture = fixture_;

            // initialize test file from copy. Root dir is bin/debug/netcoreapp2.0
            File.Copy("..\\..\\..\\..\\Artefacts\\basedb_empty.sqlite", "..\\..\\..\\..\\WorkDir\\basedb_tests.sqlite", true);
        }

        [Fact]
        public void Test()
        {
            Assert.Empty(_fixture.GetRepository<ICredentialTypeRepository>().All());
            new DatabaseInitializer().CheckAndInitialize(_fixture.DatabaseContext);
            Assert.Single(_fixture.GetRepository<ICredentialTypeRepository>().All());

        }
    }
}
