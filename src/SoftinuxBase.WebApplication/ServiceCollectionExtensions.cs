// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftinuxBase.Infrastructure;
using SoftinuxBase.Security;
using SoftinuxBase.Security.AuthorizeSetup;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.DataLayer;
using SoftinuxBase.Security.FeatureAuthorize.PolicyCode;
using SoftinuxBase.Security.UserImpersonation.AppStart;

namespace SoftinuxBase.WebApplication
{
    /// <summary>
    /// Contains the extension methods of the <see cref="IServiceCollection">IServiceCollection</see> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure : identity, application cookie, corporate logo, ExtCore, anti-forgery.
        /// It must be called inside the ConfigureServices method of the web application's Startup class
        /// in order SoftinuxBase to work properly.
        /// </summary>
        /// <typeparam name="T">Your EF DBContext.</typeparam>
        /// <param name="services_">
        /// The services collection passed to the ConfigureServices method of the web application's Startup class.
        /// </param>
        /// <param name="configuration_">Application configuration object.</param>
        /// <param name="extensionsPath_">Extensions folder location.</param>
        public static void AddSoftinuxBase<T>(this IServiceCollection services_, IConfiguration configuration_, string extensionsPath_)
            where T : DbContext
        {
            // services_.AddTransient<IUserClaimsPrincipalFactory<User>, ClaimsPrincipalFactory>();

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
                options_.Cookie.HttpOnly = true; // this must be true to prevent XSS
                options_.Cookie.SameSite = (SameSiteMode)Enum.Parse(typeof(SameSiteMode), configuration_["ConfigureApplicationCookie:Cookie.SameSite"], false);
                options_.Cookie.SecurePolicy = (CookieSecurePolicy)Enum.Parse(typeof(CookieSecurePolicy), configuration_["ConfigureApplicationCookie:Cookie.SecurePolicy"], false); // should ideally be "Always"

                options_.SlidingExpiration = true;

            });

            // 2b. Configuration for new permissions system
            services_.Configure<CookiePolicyOptions>(options_ =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options_.CheckConsentNeeded = context_ => false; // !!!!!!!!!!!!!!!!!!!!!! Turned off
                options_.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services_.AddSingleton(configuration_); // Needed for SuperAdmin setup
            services_.Configure<PermissionsSetupOptions>(configuration_.GetSection("PermissionsSetup"));

            // Register the Permission policy handlers
            services_.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services_.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            // This registers/sets up the services in these projects.
            services_.UserImpersonationRegister();

            // This enables Cookies for authentication and adds the feature and data claims to the user
            services_.ConfigureCookiesForExtraAuth();

            //// This has to come after the ConfigureCookiesForExtraAuth settings, which sets up the IAuthChanges
            //services_.ConfigureGenericServicesEntities(typeof(ApplicationStorageContext))
            //    .ScanAssemblesForDtos(Assembly.GetAssembly(typeof(ListUsersDto)))
            //    .RegisterGenericServices();

            services_.AddScoped<ApplicationStorageContext, ApplicationStorageContext>();

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