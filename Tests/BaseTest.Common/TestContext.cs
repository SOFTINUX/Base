// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BaseTest.Common
{
    /// <summary>
    /// Context for unit tests that gives access to (ExtCore) storage layer and later to fake http context.
    /// </summary>
    public class TestContext : IRequestHandler, ITestContext
    {
        /// <summary>
        /// Shared context for unit tests that setups and holds the database connection.
        /// </summary>
        /// <param name="connectionStringPath_">Connection string to use for this context</param>
        public TestContext(string connectionStringPath_)
        {
            if (string.IsNullOrWhiteSpace(connectionStringPath_))
                throw new ArgumentNullException("connectionStringPath_", "A connection string path (from appsettings.json) should be provided");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();

            StorageContextOptions storageOptions =
                new StorageContextOptions {ConnectionString = configuration[connectionStringPath_].Replace("{binDir}", Directory.GetCurrentDirectory()) };

            Storage = new Storage(GetProviderStorageContext(new TestOptions(storageOptions)));

            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddConsole(configuration.GetSection("Logging")); //log levels set in your configuration
            LoggerFactory.AddDebug(); //does all log levels

        }
        public HttpContext HttpContext { get; }
        public IStorage Storage { get; }
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// Returns the provider-specific StorageContextBase to use.
        /// </summary>
        /// <param name="options_"></param>
        /// <returns></returns>
        public virtual StorageContextBase GetProviderStorageContext(IOptions<StorageContextOptions> options_)
        {
            throw new Exception("Please provide a storage provider implementation");
        }

        private class TestOptions : IOptions<StorageContextOptions>
        {
            public TestOptions(StorageContextOptions value_)
            {
                Value = value_;
            }

            public StorageContextOptions Value { get; }
        }
    }
}
