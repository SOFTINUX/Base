// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Xunit;
using Xunit.Abstractions;

namespace BaseTest.Common
{
    /// <summary>
    /// Base class for test classes.
    /// </summary>
    public abstract class BaseTest : IClassFixture<DatabaseFixture>
    {
        /// <summary>
        /// Storage of injected object that represents a test context and that holds the DB connection.
        /// </summary>
        protected DatabaseFixture _fixture;

        /// <summary>
        /// Storage of an injected object that allows to write text to test output console.
        /// </summary>
        protected ITestOutputHelper _outputHandler;
        /// <summary>
        /// Shortcut to IStorage provided by fixture's database context.
        /// </summary>
        protected IStorage Storage { get { return _fixture.DatabaseContext.Storage; }}

    }
}
