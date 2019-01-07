// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
        /// <param name="hostingEnvironment_"></param>
        /// <param name="loggerFactory_"></param>
        /// <param name="configuration_"></param>
        /// <param name="antiForgery_"></param>
        public static void UseSoftinuxBase(this IApplicationBuilder applicationBuilder_, IHostingEnvironment hostingEnvironment_, ILoggerFactory loggerFactory_, IConfiguration configuration_, IAntiforgery antiForgery_)
        {
            // 1. Error management
            if (hostingEnvironment_.IsDevelopment())
            {
                applicationBuilder_.UseDeveloperExceptionPage();
                applicationBuilder_.UseDatabaseErrorPage();
            }
            else
            {
                applicationBuilder_.UseExceptionHandler("/Barebone/Error");
                applicationBuilder_.UseHsts();
            }

            // Security header make me happy
            applicationBuilder_.UseHsts(hsts_ => hsts_.MaxAge(365).IncludeSubdomains());
            applicationBuilder_.UseXContentTypeOptions();
            applicationBuilder_.UseReferrerPolicy(option_ => option_.NoReferrer());
            applicationBuilder_.UseXXssProtection(option_ => option_.EnabledWithBlockMode());
            applicationBuilder_.UseXfo(option_ => option_.Deny());

            applicationBuilder_.UseCsp(option_ => option_
                .BlockAllMixedContent()
                .StyleSources(s_ => s_.Self())
                .StyleSources(s_ => s_.UnsafeInline())
                .StyleSources(s_ => s_.Self().CustomSources("data:"))
                .FontSources(s_ => s_.Self())
                .FormActions(s_ => s_.Self())
                .FrameAncestors(s_ => s_.Self())
                .ImageSources(s_ => s_.Self())
                .ImageSources(s_ => s_.Self().CustomSources("data:"))
                .ScriptSources(s_ => s_.Self())
                .ScriptSources(s_ => s_.UnsafeInline()));

            // 2. Anti-forgery
            applicationBuilder_.Use(next_ => context_ =>
                {
                    string path = context_.Request.Path.Value;

                    if (
                        string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(path, "/signin", StringComparison.OrdinalIgnoreCase))
                    {
                    // The request token can be sent as a JavaScript-readable cookie,
                    // and Angular uses it by default.
                    var tokens = antiForgery_.GetAndStoreTokens(context_);
                    context_.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
                    }

                    return next_(context_);
                });

            // 3. ExtCore
            applicationBuilder_.UseExtCore();

            // 4. Static files
            applicationBuilder_.UseStaticFiles();
        }
    }
}