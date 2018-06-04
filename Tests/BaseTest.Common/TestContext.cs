// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BaseTest.Common
{
    /// <summary>
    /// Context for unit tests that gives access to (ExtCore/Identity) storage layer and later to fake http context.
    /// </summary>
    public class TestContext : IRequestHandler, ITestContext
    {
        /// <summary>
        /// The connectiong string value (not connection string path).
        /// </summary>
        protected string _connectionString;

        /// <summary>
        /// Shared context for unit tests that setups and holds the database connection.
        /// </summary>
        /// <param name="connectionStringPath_">Name of connection string from appsettings.json to use for this context</param>
        public TestContext(string connectionStringPath_)
        {
            if (string.IsNullOrWhiteSpace(connectionStringPath_))
                throw new ArgumentNullException("connectionStringPath_", "A connection string path (from appsettings.json) should be provided");

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = configBuilder.Build();

            _connectionString = configuration[connectionStringPath_].Replace("{binDir}", Directory.GetCurrentDirectory());

            var builder = GetDbContextOptionsBuilder();
            Storage = new Storage(GetProviderStorageContext(builder.Options));

            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddConsole(configuration.GetSection("Logging")); //log levels set in your configuration
            LoggerFactory.AddDebug(); //does all log levels

        }
        public HttpContext HttpContext { get; }
        public IStorage Storage { get; }
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// Returns the provider-specific IStorageContext to use.
        /// </summary>
        /// <param name="options_"></param>
        /// <returns></returns>
        public virtual IStorageContext GetProviderStorageContext(DbContextOptions<BaseTestDbContext> options_)
        {
            throw new Exception("Please provide a storage provider implementation");
        }

        public virtual DbContextOptionsBuilder<BaseTestDbContext> GetDbContextOptionsBuilder()
        {
            throw new Exception("Please provide a DbContextOptionsBuilder");
        }
    }
}
