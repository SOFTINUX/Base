// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using ExtCore.WebApplication.Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Security;
using Security.Data.Entities;
using Serilog;

namespace WebApplication
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly string _extensionsPath;

        public Startup(IConfiguration configuration_, IHostingEnvironment hostingEnvironment_)
        {
            Configuration = configuration_;
            _extensionsPath = hostingEnvironment_.ContentRootPath + Configuration["Extensions:Path"].Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }

        public void ConfigureServices(IServiceCollection services_)
        {
            services_.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            // Note: AddScoped : for services based on EF (once per request),
            // other values : AddTransient (stateless), AddSingleton (avoids to implement singleton pattern ourselves)

            services_.AddSingleton(Configuration);

            /* services_.Configure<StorageContextOptions>(options_ =>
                {
                    options_.ConnectionString = Configuration["ConnectionStrings:Default"];
                    //options_.MigrationsAssembly = typeof(DesignTimeStorageContextFactory).GetTypeInfo().Assembly.FullName;
                }
            ); */

            // Configure Identity
            services_.AddIdentity<Security.Data.Entities.User, IdentityRole<string>>(options =>
                {
                    // Configure identity options here.
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                })
               .AddEntityFrameworkStores<ApplicationStorageContext>()
               .AddDefaultTokenProviders(); // Tell Identity which EF DbContext to use

               // configure the application cookie
            services_.ConfigureApplicationCookie( options =>
            {
                // override the default event
               /*  options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToAccessDenied = ReplaceRedirectorWithStatusCode(HttpStatusCode.Forbidden),
                    OnRedirectToLogin = ReplaceRedirectorWithStatusCode(HttpStatusCode.Unauthorized)
                }; */

                // customize other stuff as needed
                options.LoginPath = Configuration["ConfigureApplicationCookie:LoginPath"];
                options.LogoutPath = Configuration["ConfigureApplicationCookie:LogoutPath"];
                options.Cookie.Name = Configuration["ConfigureApplicationCookie:Cookie.Name"] + Configuration["Corporate:Name"];
                options.Cookie.HttpOnly = true; //this must be true to prevent XSS
                options.Cookie.SameSite = (SameSiteMode)Enum.Parse(typeof(SameSiteMode), Configuration["ConfigureApplicationCookie:Cookie.SameSite"], false);
                options.Cookie.SecurePolicy = (CookieSecurePolicy)Enum.Parse(typeof(CookieSecurePolicy), Configuration["ConfigureApplicationCookie:Cookie.SecurePolicy"], false); //should ideally be "Always"

                options.SlidingExpiration = true;
            });

            services_.AddDbContext<ApplicationStorageContext>(options =>
                {
                    options.UseSqlite(Configuration["ConnectionStrings:Default"]);
                },
                ServiceLifetime.Scoped
            );

            services_.Configure<CorporateConfiguration>(options_ =>
                {
                    options_.Name = Configuration.GetValue<string>("Corporate:Name");
                    options_.BrandLogo = Configuration.GetValue<string>("Corporate:BrandLogo");
                }
            );

            services_.AddScoped<IUserClaimsPrincipalFactory<User>, ClaimsPrincipalFactory>();

            // Register database-specific storage context implementation.
            // Necessary for IStorage service registration to fully work (see AddAuthorizationPolicies).
            services_.AddScoped<IStorageContext, ApplicationStorageContext>();
            services_.AddScoped<IStorage, Storage>();

            //DesignTimeStorageContextFactory.Initialize(services_.BuildServiceProvider());

            services_.AddExtCore(_extensionsPath);

            // register for DI to for in REST controller
            services_.AddScoped<IServiceCollection, ServiceCollection>();

        }

        public void Configure(IApplicationBuilder applicationBuilder_, IHostingEnvironment hostingEnvironment_, ILoggerFactory loggerFactory_, IConfiguration configuration_)
        {
            if (hostingEnvironment_.IsDevelopment())
            {
                applicationBuilder_.UseDeveloperExceptionPage();
                applicationBuilder_.UseDatabaseErrorPage();
                //applicationBuilder_.UseBrowserLink();
            }

            // call ConfigureLogger in a centralized place in the code
            // so that we configure the logger factory provided by .NET Core with our configuration in Logging class.
            Logging.ConfigureLogger(loggerFactory_, configuration_);
            //set it as the primary LoggerFactory to use everywhere
            Logging.LoggerFactory = loggerFactory_;

#if DEBUG
            Log.Information("#######################################################");
            Log.Information("webroot path: " + hostingEnvironment_.WebRootPath + "\n" + "Content Root path: " + hostingEnvironment_.ContentRootPath);
            Log.Information("#######################################################");
#endif

            applicationBuilder_.UseAuthentication();
            applicationBuilder_.UseExtCore();

            System.Console.WriteLine("PID= " + System.Diagnostics.Process.GetCurrentProcess().Id);
        }
    }
}