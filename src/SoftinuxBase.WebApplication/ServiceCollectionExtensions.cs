// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftinuxBase.Infrastructure;
using SoftinuxBase.Security;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.WebApplication
{
    /// <summary>
    /// Contains the extension methods of the <see cref="IServiceCollection">IServiceCollection</see> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure : logging, identity, application cookie, corporate logo, ExtCore, anti-forgery.
        /// It must be called inside the ConfigureServices method of the web application's Startup class
        /// in order SoftinuxBase to work properly.
        /// </summary>
        /// <param name="services_">
        /// The services collection passed to the ConfigureServices method of the web application's Startup class.
        /// </param>
        /// <param name="configuration_">
        /// </param>
        /// <param name="extensionsPath_">
        /// </param>
        public static void AddSoftinuxBase<T>(this IServiceCollection services_, IConfiguration configuration_, string extensionsPath_) where T : DbContext
        {
            services_.AddTransient<IUserClaimsPrincipalFactory<User>, ClaimsPrincipalFactory>();

            // Configure Identity (cannot move this to Security extension because of ApplicationStorageContext).
            services_.AddIdentity<User, IdentityRole<string>>(options_ =>
            {
                // 1. Configure identity options
                // TODO use IOptions pattern instead of Configuration.GetValue<>
                // See also https://codereview.stackexchange.com/questions/185297/adding-extension-methods-to-iservicecollection-in-asp-net-core
                options_.Password.RequireDigit = configuration_.GetValue<bool>("PasswordStrategy:Password.RequireDigit");
                options_.Password.RequiredLength = configuration_.GetValue<int>("PasswordStrategy:Password.RequiredLength");
                options_.Password.RequireLowercase = configuration_.GetValue<bool>("PasswordStrategy:Password.RequireLowercase");
                options_.Password.RequireNonAlphanumeric = configuration_.GetValue<bool>("PasswordStrategy:Password.RequireNonAlphanumeric");
                options_.Password.RequireUppercase = configuration_.GetValue<bool>("PasswordStrategy:Password.RequireUppercase");

                options_.Lockout.AllowedForNewUsers = configuration_.GetValue<bool>("LockoutUser:Lockout.AllowedForNewUsers");
                options_.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(configuration_.GetValue<int>("LockoutUser:Lockout.DefaultLockoutTimeSpan"));
                options_.Lockout.MaxFailedAccessAttempts = configuration_.GetValue<int>("LockoutUser:Lockout.MaxFailedAccessAttempts");

                options_.SignIn.RequireConfirmedEmail = configuration_.GetValue<bool>("SignIn:RequireConfirmedEmail");
                options_.SignIn.RequireConfirmedPhoneNumber = configuration_.GetValue<bool>("SignIn:RequireConfirmedPhoneNumber");

                options_.User.RequireUniqueEmail = configuration_.GetValue<bool>("ValidateUser:options.User.RequireUniqueEmail");
            })
            .AddEntityFrameworkStores<T>()
            .AddDefaultTokenProviders(); // Tell Identity which EF DbContext to use

            // 2. Configure the application cookie
            services_.ConfigureApplicationCookie(options_ =>
            {
                // override the default event
                /*  options.Events = new CookieAuthenticationEvents
                 {
                     OnRedirectToAccessDenied = ReplaceRedirectorWithStatusCode(HttpStatusCode.Forbidden),
                     OnRedirectToLogin = ReplaceRedirectorWithStatusCode(HttpStatusCode.Unauthorized)
                 }; */

                // customize other stuff as needed
                options_.LoginPath = configuration_["ConfigureApplicationCookie:LoginPath"];
                options_.LogoutPath = configuration_["ConfigureApplicationCookie:LogoutPath"];
                options_.Cookie.Name = configuration_["ConfigureApplicationCookie:Cookie.Name"] + configuration_["Corporate:Name"];
                options_.Cookie.HttpOnly = true; //this must be true to prevent XSS
                options_.Cookie.SameSite = (SameSiteMode)Enum.Parse(typeof(SameSiteMode), configuration_["ConfigureApplicationCookie:Cookie.SameSite"], false);
                options_.Cookie.SecurePolicy = (CookieSecurePolicy)Enum.Parse(typeof(CookieSecurePolicy), configuration_["ConfigureApplicationCookie:Cookie.SecurePolicy"], false); //should ideally be "Always"

                options_.SlidingExpiration = true;
            });

            // 3. Configure the corporate logo
            services_.Configure<CorporateConfiguration>(options_ =>
            {
                options_.Name = configuration_.GetValue<string>("Corporate:Name");
                options_.BrandLogo = configuration_.GetValue<string>("Corporate:BrandLogo");
                options_.LoginBackgroundImage = configuration_.GetValue<string>(@"Corporate:LoginBackgroundImage");
                options_.RegisterNewUsers = configuration_.GetValue<bool>("Corporate:RegisterNewUsers");
            });

            // 4. ExtCore
            services_.AddScoped<IStorage, Storage>();

            services_.AddExtCore(extensionsPath_);

            // register for DI to work with Security.ServiceConfiguration
            // TODO check that this is really needed
            services_.AddScoped<IServiceCollection, ServiceCollection>();

            // 5. Anti-forgery
            services_.AddAntiforgery(options_ =>
            {
                options_.HeaderName = "X-XSRF-TOKEN";
                options_.SuppressXFrameOptionsHeader = false;
            });
        }
    }
}