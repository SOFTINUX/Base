using System;
using System.IO;
using System.Linq;
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

        public UserManager<User> UserManager { get; private set; }
        public RoleManager<IdentityRole<string>> RoleManager { get; private set;}

        public DatabaseFixture()
        {
            var services = new ServiceCollection();
            _serviceProvider = services.BuildServiceProvider();
            ConfigureServices(services);
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

            services.AddExtCore("");

            // Assign shortcuts accessors to registered components
            //Storage = _serviceProvider.GetRequiredService<IStorage>();

            var test = services.FirstOrDefault(s => s.ServiceType == typeof(IStorage));
            var test2 = services.FirstOrDefault(s => s.ServiceType == typeof(UserManager<User>));
            var test3 = services.FirstOrDefault(s => s.ServiceType == typeof(RoleManager<IdentityRole<string>>));

            //UserManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            //RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole<string>>>();
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