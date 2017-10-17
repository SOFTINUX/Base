using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecurityTest.Util;
using Serilog;

namespace SecurityTest
{
    /// <summary>
    /// Context for unit tests that gives access to storage layer and later to fake http context.
    /// </summary>
    public class TestContext : IRequestHandler
    {
        public TestContext()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();

            StorageContextOptions storageOptions =
                new StorageContextOptions {ConnectionString = configuration["ConnectionStrings:Default"].Replace("{binDir}", Directory.GetCurrentDirectory()) };

            Storage = new Storage(GetProviderStorageContext(new TestOptions(storageOptions)));

            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddConsole(configuration.GetSection("Logging")); //log levels set in your configuration
            LoggerFactory.AddDebug(); //does all log levels

            // WIP use Serilog logger to write to a file
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .WriteTo.RollingFile(Path.Combine(Directory.GetCurrentDirectory(), "log-{Date}.txt"))
            //    .CreateLogger();
            //LoggerFactory.AddSerilog();

        }
        public HttpContext HttpContext { get; }
        public IStorage Storage { get; }
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// Returns the provider-specific StorageContextBase to use.
        /// </summary>
        /// <param name="options_"></param>
        /// <returns></returns>
        public StorageContextBase GetProviderStorageContext(IOptions<StorageContextOptions> options_)
            {
                return new TestStorageContextBase(options_);
            } 

        public class TestOptions : IOptions<StorageContextOptions>
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
}
