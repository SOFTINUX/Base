// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using BaseTest.Util;
using Xunit.Abstractions;

namespace BaseTest
{
    public abstract class BaseTest
    {
        /// <summary>
        /// Storage of injected object that represents a test context and that holds the DB connection.
        /// </summary>
        protected DatabaseFixture _fixture;

        /// <summary>
        /// Storage of an injected object that allows to write text to test output console.
        /// </summary>
        protected ITestOutputHelper _outputHandler;
    }
}
