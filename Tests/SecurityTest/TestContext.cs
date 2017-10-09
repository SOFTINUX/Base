using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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

        }
        public HttpContext HttpContext { get; }
        public IStorage Storage { get; }

        /// <summary>
        /// Returns the provider-specific StorageContextBase to use.
        /// </summary>
        /// <param name="options_"></param>
        /// <returns></returns>
        public StorageContextBase GetProviderStorageContext(IOptions<StorageContextOptions> options_)
            {
                return new ExtCore.Data.EntityFramework.Sqlite.StorageContext(options_);
            } 

        public class TestOptions : IOptions<StorageContextOptions>
        {
            public TestOptions(StorageContextOptions value_)
            {
                Value = value_;
            }

            public StorageContextOptions Value { get; }
        }
    }
}
