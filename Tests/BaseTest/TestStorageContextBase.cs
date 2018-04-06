// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.EntityFramework;
using Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BaseTest
{
    /// <summary>
    /// Provider-specific storage context to use, with sensitive data logging enabled for unit tests debugging.
    /// </summary>
    public class TestStorageContextBase : ExtCore.Data.EntityFramework.Sqlite.StorageContext
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