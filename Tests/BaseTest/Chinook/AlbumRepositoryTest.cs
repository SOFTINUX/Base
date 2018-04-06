// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using BaseTest.Util;
using Chinook.Data.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace BaseTest.Chinook
{
    [Collection("Database collection")]
    public class AlbumRepositoryTest : BaseTest
    {
        public AlbumRepositoryTest(DatabaseFixture fixture_, ITestOutputHelper outputHandler_)
        {
            _fixture = fixture_;
            _outputHandler = outputHandler_;
        }

        /// <summary>
        /// Test that we load data correctly, using a non-empty database.
        /// </summary>
        [Fact, TestPriority(1)]
        public void TestAll()
        {
            // FIXME: null reference
            var test = _fixture.GetRepository<IAlbumRepository>().All();
            Assert.NotEmpty(_fixture.GetRepository<IAlbumRepository>().All());
        }
    }
}
