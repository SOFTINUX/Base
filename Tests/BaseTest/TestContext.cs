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
using Security.Data.EntityFramework.Util;

namespace BaseTest
{
    /// <summary>
    /// Context for unit tests that gives access to storage layer and later to fake http context.
    /// </summary>
    public class TestContext : IRequestHandler
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
        private StorageContextBase GetProviderStorageContext(IOptions<StorageContextOptions> options_)
        {
            return new TestStorageContextBase(options_);
        }

        private class TestOptions : IOptions<StorageContextOptions>
        {
            public TestOptions(StorageContextOptions value_)
            {
                Value = value_;
            }

            public StorageContextOptions Value { get; }
        }

        /// <summary>
        /// Provider-specific storage context to use, with sensitive data logging enabled for unit tests debugging.
        /// </summary>
        private class TestStorageContextBase : ExtCore.Data.EntityFramework.Sqlite.StorageContext
        {
            public TestStorageContextBase(IOptions<StorageContextOptions> options_) : base(options_)
            {
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder_)
            {
                ILoggerFactory loggerFactory = new LoggerFactory();
                loggerFactory.AddProvider(new EfLoggerProvider());
                base.OnConfiguring(optionsBuilder_.EnableSensitiveDataLogging().UseLoggerFactory(loggerFactory));
            }
        }
    }
}
