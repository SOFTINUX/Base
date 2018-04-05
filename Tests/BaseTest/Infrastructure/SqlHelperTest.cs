// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using BaseTest.Util;
using Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace BaseTest.Infrastructure
{
    [Collection("Database collection")]
    public  class SqlHelperTest : BaseTest
    {
        public SqlHelperTest(DatabaseFixture fixture_, ITestOutputHelper outputHandler_)
        {
            _fixture = fixture_;
            _outputHandler = outputHandler_;

        }

        /// <summary>
        /// Test that we correctly determine the EF current provider from database context.
        /// </summary>
        [Fact, TestPriority(1)]
        public void TestSqlite()
        {
            SqlHelper sqlHelper = new SqlHelper(_fixture.DatabaseContext.Storage, ((TestContext)_fixture.DatabaseContext).LoggerFactory);
            Assert.Equal(SqlHelper.ProviderCode.Sqlite, sqlHelper.GetProvider());

        }
    }
}
