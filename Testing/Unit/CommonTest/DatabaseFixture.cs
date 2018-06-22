using System;
using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Security.Data.Entities;

namespace CommonTest
{
    public class DatabaseFixture : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        public IConfiguration Configuration {get; private set;}
        public IStorage Storage { get; private set; }
        public ILoggerFactory LoggerFactory { get; private set; }

        public DatabaseFixture()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            Configuration = LoadConfiguration();

            // Register UserManager & RoleManager
            services.AddIdentity<User, IdentityRole<string>>()
               .AddEntityFrameworkStores<ApplicationStorageContext>()
               .AddDefaultTokenProviders();

            // UserManager & RoleManager require logging and HttpContext dependencies
            services.AddLogging();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<ApplicationStorageContext>(options_ =>
                {
                    options_.UseSqlite(Configuration["ConnectionStrings:Default"]);
                });

            // Register database-specific storage context implementation.
            services.AddScoped<IStorageContext, ApplicationStorageContext>();
            services.AddScoped<IStorage, Storage>();

            services.AddExtCore(string.Empty);

            // Assign shortcuts accessors to registered components
            Storage = _serviceProvider.GetRequiredService<IStorage>();
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

/// <summary>
/// For use by DbContextFactory.
/// </summary>
/// <returns></returns>
        public static DbContextOptionsBuilder<ApplicationStorageContext> GetDbContextOptionsBuilder()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationStorageContext>();
            // Configure connection string
            var configuration = LoadConfiguration();
            optionsBuilder.UseSqlite(configuration["ConnectionStrings:Default"]);
            return optionsBuilder;
        }

    }
}