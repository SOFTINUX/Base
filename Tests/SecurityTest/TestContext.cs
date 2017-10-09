using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SecurityTest
{
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

            Storage = new Storage(new TestDbContext(new TestOptions(storageOptions)));

        }
        public HttpContext HttpContext { get; }
        public IStorage Storage { get; }

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
