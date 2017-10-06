using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseSqlite(configuration["ConnectionStrings:Default"]);
            // TODO FIXME have a deeper look at how Extcore works
            Storage = new Storage(new TestDbContext(optionsBuilder.Options));

        }
        public HttpContext HttpContext { get; }
        public IStorage Storage { get; }
    }
}
