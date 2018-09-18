// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

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
        public IConfiguration Configuration { get; private set; }
        public IStorage Storage { get; private set; }
        public ILoggerFactory LoggerFactory { get; private set; }

        public UserManager<User> UserManager { get; private set; }
        public RoleManager<IdentityRole<string>> RoleManager { get; private set; }

        public DatabaseFixture()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // Assign shortcuts accessors to registered components
            UserManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole<string>>>();

            // Storage: create using registered db context / storage context
            Storage = new Storage(_serviceProvider.GetRequiredService<IStorageContext>());
        }

        /// <summary>
        /// At runtime, with a SqLite database, the current directory is where the dll live
        /// (bin/Debug/netcoreapp2.1).
        /// Then we must adapt the path from configuration.
        /// Override this method to redefine your own connection string when you need.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetConnectionString()
        {
            return Configuration["ConnectionStrings:Default"].Replace("Data Source=", "Data Source=../../../");
        }

        private void ConfigureServices(IServiceCollection services)
        {
            Configuration = LoadConfiguration();

            // DbContext/IStorageContext
            services.AddDbContext<ApplicationStorageContext>(options_ =>
                {
                    options_.UseSqlite(GetConnectionString());
                });

            // Register UserManager & RoleManager
            services.AddIdentity<User, IdentityRole<string>>()
               .AddEntityFrameworkStores<ApplicationStorageContext>()
               .AddDefaultTokenProviders();

            // UserManager & RoleManager require logging and HttpContext dependencies
            services.AddLogging();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            // Register database-specific storage context implementation.
            services.AddScoped<IStorageContext, ApplicationStorageContext>();
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