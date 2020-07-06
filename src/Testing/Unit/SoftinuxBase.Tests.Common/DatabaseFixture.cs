// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftinuxBase.Security.AuthorizeSetup;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.FeatureAuthorize.PolicyCode;
using SoftinuxBase.Security.UserImpersonation.AppStart;
using SoftinuxBase.WebApplication;

namespace SoftinuxBase.Tests.Common
{
    public sealed class DatabaseFixture : IDisposable
    {
        public IStorage Storage { get; }
        public RoleManager<IdentityRole<string>> RoleManager { get; }
        private IConfiguration Configuration { get; set; }

        public DatabaseFixture()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Assign shortcuts accessors to registered components
            RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<string>>>();
            Storage = serviceProvider.GetRequiredService<IStorage>();
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

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        /// <summary>
        /// At runtime, with a SqLite database, the current directory is where the dll live
        /// (bin/Debug/netcoreapp...).
        /// Then we must adapt the path from configuration.
        /// Override this method to redefine your own connection string when you need.
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            return Configuration["ConnectionStrings:Default"].Replace("Data Source=", "Data Source=../../../");
        }

        private void ConfigureServices(IServiceCollection services_)
        {
            Configuration = LoadConfiguration();
            services_.AddSingleton(Configuration);

            // SoftinuxBase/db context
            services_.AddSoftinuxBase<ApplicationStorageContext>(Configuration, ".");

            // Which database provider to use : Sqlite
            services_.AddDbContext<ApplicationStorageContext>(options_ =>
            {
                options_.UseSqlite(GetConnectionString());
            });

            // Register database-specific storage context implementation.
            services_.AddScoped<IStorageContext, ApplicationStorageContext>();

            // // 1. Register UserManager & RoleManager
            //  services_.AddIdentity<User, IdentityRole<string>>()
            //      .AddEntityFrameworkStores<ApplicationStorageContext>()
            //      .AddDefaultTokenProviders();
            //
            //  // UserManager & RoleManager require logging and HttpContext dependencies
            //  services_.AddLogging();
            //  services_.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //
            //  // 2. New permission system
            //  services_.Configure<PermissionsSetupOptions>(Configuration.GetSection("PermissionsSetup"));
            //  // This has to come after the ConfigureCookiesForExtraAuth settings, which sets up the IAuthChanges
            //  services_.AddScoped<ApplicationStorageContext, ApplicationStorageContext>();
            //
            //  // 3. Add ExtCore
            //  services_.AddScoped<IStorage, Storage>();
            //  services_.AddExtCore();
            //
            //  // Register database-specific storage context implementation.
            //  services_.AddScoped<IStorageContext, ApplicationStorageContext>();
            //  
            //  // Register the Permission policy handlers
            //  services_.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            //  services_.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            //
            //  // This registers/sets up the services in these projects.
            //  services_.UserImpersonationRegister();
            //
            //  // This enables Cookies for authentication and adds the feature and data claims to the user
            //  services_.ConfigureCookiesForExtraAuth();
            //
            //  // 4. DbContext/IStorageContext
            //  services_.AddDbContext<ApplicationStorageContext>(options_ =>
            //  {
            //      options_.UseSqlite(GetConnectionString());
            //  });
        }
    }
}