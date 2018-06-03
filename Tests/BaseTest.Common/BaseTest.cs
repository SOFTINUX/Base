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
        /// Storage of injected IStorage, if the derived class get some injected
        /// </summary>
        protected IStorage _storage;
        /// <summary>
        /// Storage of injected Identity UserManager, if the derived class get some injected
        /// </summary>
        protected UserManager<Security.Data.Entities.User> _userManager;

        /// <summary>
        /// Storage of injected Identity role manager, if the derived class get some injected
        /// </summary>
        protected RoleManager<IdentityRole<string>> _roleManager;
    }
}
