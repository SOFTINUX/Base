// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Net;
using System.Threading.Tasks;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace SoftinuxBase.Security.ServiceConfiguration
{
    /// <summary>
    /// Configuration of application services to activate authenticated access.
    /// </summary>
    internal class ConfigureAuthentication : IConfigureServicesAction
    {
        public int Priority => 200;

        public void Execute(IServiceCollection serviceCollection_, IServiceProvider serviceProvider_)
        {
        }

        private static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirectorWithStatusCode(
            HttpStatusCode statusCode_) => context_ =>
        {
            // Adapted from https://stackoverflow.com/questions/42030137/suppress-redirect-on-api-urls-in-asp-net-core
            context_.Response.StatusCode = (int)statusCode_;
            return Task.CompletedTask;
        };
    }
}
