// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using BaseTest.Common.XunitTools;
using Chinook.Data.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace BaseTest.Chinook
{
    [Collection("Database collection")]
    public class AlbumRepositoryTest : Common.BaseTest
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
            var albumList = _fixture.GetRepository<IAlbumRepository>().All();
            Assert.NotEmpty(albumList);
            Assert.NotNull(albumList.FirstOrDefault(a_ => a_.Title == "Chemical Wedding"));
        }
    }
}
