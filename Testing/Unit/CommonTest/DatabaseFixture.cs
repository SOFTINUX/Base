using System;
using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommonTest
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration["ConnectionStrings:Default"];

            StorageContextOptions storageOptions =
                new StorageContextOptions {ConnectionString = connectionString};

            Storage = new Storage(GetProviderSpecificStorageContext(new TestStorageContextOptions(storageOptions)));

            // ... initialize data in the test database ...
        }

        /// <summary>
        /// Instantiate the provider-specif cstorage context. Override this to use another storage context than the Sqlite's one.
        /// </summary>
        /// <param name="options_"></param>
        /// <returns></returns>
        public virtual IStorageContext GetProviderSpecificStorageContext(IOptions<StorageContextOptions> options_)
        {
            return new SqliteStorageContext(options_);
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

        public IStorage Storage { get; }
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// Utility class to use the IOptions pattern as required by IStorage implementations constructors.
        /// </summary>
        /// <typeparam name="StorageContextOptions"></typeparam>
        private class TestStorageContextOptions : IOptions<StorageContextOptions>
        {
            public TestStorageContextOptions(StorageContextOptions value_)
            {
                Value = value_;
            }

            public StorageContextOptions Value { get; }
        }
    }
}