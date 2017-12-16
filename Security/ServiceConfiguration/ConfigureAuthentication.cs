// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Net;
using System.Threading.Tasks;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Security.Data.Entities;
using Security.Data.EntityFramework.Sqlite;

namespace Security.ServiceConfiguration
{
    /// <summary>
    /// Configuration of application services to activate authenticated access
    /// </summary>
    public class ConfigureAuthentication : IConfigureServicesAction
    {
        public int Priority => 200;

        public void Execute(IServiceCollection serviceCollection_, IServiceProvider serviceProvider_)
        {
            // Configure Identity
            serviceCollection_.AddIdentity<User, Role>(options =>
                {
                    // Configure identity options here.
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                })
                .AddEntityFrameworkStores<ApplicationStorageContext>(); // Tell Identity which EF DbContext to use

            serviceCollection_.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options_ =>
                    {
                        // Override the default events (By default Identity performs a 302 redirect to a login page for unauthenticated or unauthorized requests)
                        options_.Events = new CookieAuthenticationEvents
                        {
                            OnRedirectToAccessDenied = ReplaceRedirectorWithStatusCode(HttpStatusCode.Forbidden),
                            OnRedirectToLogin = ReplaceRedirectorWithStatusCode(HttpStatusCode.Unauthorized)
                        };
                        options_.AccessDeniedPath = "/account/accessdenied";
                        options_.LoginPath = "/account/signin";
                        options_.LogoutPath = "/account/signout";
                        options_.ReturnUrlParameter = "next";
                        options_.ExpireTimeSpan = TimeSpan.FromDays(7);
                    }
                );
        }

        static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirectorWithStatusCode(
            HttpStatusCode statusCode_) => context_ =>
        {
            // Adapted from https://stackoverflow.com/questions/42030137/suppress-redirect-on-api-urls-in-asp-net-core
            context_.Response.StatusCode = (int)statusCode_;
            return Task.CompletedTask;
        };
    }
}
