// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

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
            services_.AddLogging(loggingBuilder_ => loggingBuilder_.AddSerilog(dispose: true));

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
            services_.AddIdentity<User, IdentityRole<string>>(options_ =>
            {
                // Configure identity options here.
                options_.Password.RequireDigit = Configuration.GetValue<bool>("PasswordStrategy:Password.RequireDigit");
                options_.Password.RequiredLength = Configuration.GetValue<int>("PasswordStrategy:Password.RequiredLength");
                options_.Password.RequireLowercase = Configuration.GetValue<bool>("PasswordStrategy:Password.RequireLowercase");
                options_.Password.RequireNonAlphanumeric = Configuration.GetValue<bool>("PasswordStrategy:Password.RequireNonAlphanumeric");
                options_.Password.RequireUppercase = Configuration.GetValue<bool>("PasswordStrategy:Password.RequireUppercase");

                options_.Lockout.AllowedForNewUsers = Configuration.GetValue<bool>("LockoutUser:Lockout.AllowedForNewUsers");
                options_.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Configuration.GetValue<int>("LockoutUser:Lockout.DefaultLockoutTimeSpan"));
                options_.Lockout.MaxFailedAccessAttempts = Configuration.GetValue<int>("LockoutUser:Lockout.MaxFailedAccessAttempts");

                options_.SignIn.RequireConfirmedEmail = Configuration.GetValue<bool>("SignIn:RequireConfirmedEmail");
                options_.SignIn.RequireConfirmedPhoneNumber = Configuration.GetValue<bool>("SignIn:RequireConfirmedPhoneNumber");

                options_.User.RequireUniqueEmail = Configuration.GetValue<bool>("ValidateUser:options.User.RequireUniqueEmail");
            })
            .AddEntityFrameworkStores<ApplicationStorageContext>()
            .AddDefaultTokenProviders(); // Tell Identity which EF DbContext to use

            // configure the application cookie
            services_.ConfigureApplicationCookie(options_ =>
            {
                // override the default event
               /*  options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToAccessDenied = ReplaceRedirectorWithStatusCode(HttpStatusCode.Forbidden),
                    OnRedirectToLogin = ReplaceRedirectorWithStatusCode(HttpStatusCode.Unauthorized)
                }; */

                // customize other stuff as needed
                options_.LoginPath = Configuration["ConfigureApplicationCookie:LoginPath"];
                options_.LogoutPath = Configuration["ConfigureApplicationCookie:LogoutPath"];
                options_.Cookie.Name = Configuration["ConfigureApplicationCookie:Cookie.Name"] + Configuration["Corporate:Name"];
                options_.Cookie.HttpOnly = true; //this must be true to prevent XSS
                options_.Cookie.SameSite = (SameSiteMode)Enum.Parse(typeof(SameSiteMode), Configuration["ConfigureApplicationCookie:Cookie.SameSite"], false);
                options_.Cookie.SecurePolicy = (CookieSecurePolicy)Enum.Parse(typeof(CookieSecurePolicy), Configuration["ConfigureApplicationCookie:Cookie.SecurePolicy"], false); //should ideally be "Always"

                options_.SlidingExpiration = true;
            });

            services_.AddDbContext<ApplicationStorageContext>(options_ =>
            {
                options_.UseSqlite(Configuration["ConnectionStrings:Default"]);
            });

            services_.Configure<CorporateConfiguration>(options_ =>
            {
                options_.Name = Configuration.GetValue<string>("Corporate:Name");
                options_.BrandLogo = Configuration.GetValue<string>("Corporate:BrandLogo");
            });

            services_.AddScoped<IUserClaimsPrincipalFactory<User>, ClaimsPrincipalFactory>();

            // Register database-specific storage context implementation.
            services_.AddScoped<IStorageContext, ApplicationStorageContext>();
            services_.AddScoped<IStorage, Storage>();

            services_.AddExtCore(_extensionsPath);

            // register for DI to work with Security.ServiceConfiguration.ConfigureAuthentication
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
            else{
                applicationBuilder_.UseExceptionHandler("/Barebone/Error");
                applicationBuilder_.UseHsts();
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

            applicationBuilder_.UseExtCore();
            applicationBuilder_.UseStaticFiles();

            // More configuration like "applicationBuilder_.Use..." in Security.ServiceConfiguration.*

            Console.WriteLine("PID= " + System.Diagnostics.Process.GetCurrentProcess().Id);
        }
    }
}