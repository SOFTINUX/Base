// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Net;
using System.Threading.Tasks;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security.ServiceConfiguration
{
    /// <summary>
    /// Action that must be executed inside the ConfigureServices method of a web application's Startup class:
    ///
    /// Configuration of application services for Identity.
    /// <remarks>This class is Internal to SoftinuxBase.Security.</remarks>
    /// </summary>
    internal class ConfigureServicesAction : IConfigureServicesAction
    {
        public int Priority => 200;

        /// <summary>
        /// TODO DOCUMENT ME
        /// </summary>
        /// <param name="serviceCollection_"></param>
        /// <param name="serviceProvider_"></param>
        public void Execute(IServiceCollection serviceCollection_, IServiceProvider serviceProvider_)
        {
            serviceCollection_.AddScoped<IUserClaimsPrincipalFactory<User>, ClaimsPrincipalFactory>();
        }

        /// <summary>
        /// TODO DOCUMENT ME
        /// </summary>
        /// <param name="statusCode_"></param>
        /// <returns></returns>
        private static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirectorWithStatusCode(
            HttpStatusCode statusCode_) => context_ =>
        {
            // Adapted from https://stackoverflow.com/questions/42030137/suppress-redirect-on-api-urls-in-asp-net-core
            context_.Response.StatusCode = (int)statusCode_;
            return Task.CompletedTask;
        };
    }
}
