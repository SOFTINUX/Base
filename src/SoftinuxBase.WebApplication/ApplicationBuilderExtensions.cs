// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Infrastructure;

namespace SoftinuxBase.WebApplication
{
    /// <summary>
    /// Contains the extension methods of the <see cref="IApplicationBuilder">IApplicationBuilder</see> interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Executes the Configure actions that configure: error management, logging, anti-forgery, ExtCore, static files.
        /// It must be called inside the Configure method
        /// of the web application's Startup class in order SoftinuxBase to work properly.
        /// </summary>
        /// <param name="applicationBuilder_">
        /// The application builder passed to the Configure method of the web application's Startup class.
        /// </param>
        /// <param name="hostingEnvironment_">
        /// </param>
        /// <param name="loggerFactory_">
        /// </param>
        /// </param>
        /// <param name="configuration_">
        /// </param>
        /// <param name="antiForgery_">
        /// </param>
        public static void UseSoftinuxBase(this IApplicationBuilder applicationBuilder_, IHostingEnvironment hostingEnvironment_, ILoggerFactory loggerFactory_,
             IConfiguration configuration_, IAntiforgery antiForgery_)
        {
            // 1. Error management
            if (hostingEnvironment_.IsDevelopment())
            {
                applicationBuilder_.UseDeveloperExceptionPage();
                applicationBuilder_.UseDatabaseErrorPage();
                //applicationBuilder_.UseBrowserLink();
            }
            else
            {
                applicationBuilder_.UseExceptionHandler("/Barebone/Error");
                applicationBuilder_.UseHsts();
            }

            // 2. Anti-forgery
            applicationBuilder_.Use(next => context =>
                {
                    string path = context.Request.Path.Value;

                    if (
                        string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(path, "/signin", StringComparison.OrdinalIgnoreCase))
                    {
                    // The request token can be sent as a JavaScript-readable cookie,
                    // and Angular uses it by default.
                    var tokens = antiForgery_.GetAndStoreTokens(context);
                        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                            new CookieOptions() { HttpOnly = false });
                    }

                    return next(context);
                });
            // 3. ExtCore
            applicationBuilder_.UseExtCore();

            // 4. Static files
            applicationBuilder_.UseStaticFiles();
        }
    }
}