using System;
using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Security.Data.Entities;

namespace CommonTest
{
    public class DatabaseFixture : IDisposable
    {
        private IConfiguration Configuration { get; set; }
        private readonly IServiceProvider _serviceProvider;

        public DatabaseFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            string connectionString = Configuration["ConnectionStrings:Default"];

            StorageContextOptions storageOptions =
                new StorageContextOptions { ConnectionString = connectionString };

            Storage = new Storage(GetProviderSpecificStorageContext(new TestStorageContextOptions(storageOptions)));

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            // services.AddEntityFramework()
            //     .AddSqlServer()
            //     .AddDbContext<ApplicationDbContext>(options =>
            //         options.UseSqlServer(connectionString));

            // Register UserManager & RoleManager
            services.AddIdentity<User, IdentityRole<string>>()
               .AddEntityFrameworkStores<ApplicationStorageContext>()
               .AddDefaultTokenProviders();

            // UserManager & RoleManager require logging and HttpContext dependencies
            services.AddLogging();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Instantiate the provider-specific storage context. Override this to use another storage context than the Sqlite's one.
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