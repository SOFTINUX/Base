using ExtCore.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SecurityTest
{
    internal class TestDbContext : DbContext, IStorageContext
    {
        internal static IConfigurationRoot Configuration { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options_)
            :base(options_)
        { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder_)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json");

        //    Configuration = builder.Build();
        //    optionsBuilder_.UseSqlite(Configuration["ConnectionStrings:Default"]);
        //}
    }
}
