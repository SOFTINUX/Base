// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;

namespace SoftinuxBase.Security.ServiceConfiguration
{
    /// <summary>
    /// Action that must be executed inside the Configure method of a web application's Startup class:
    ///
    /// Activates the authentication.
    /// </summary>
    public class ConfigureAction : IConfigureAction
    {
        /// <inheritdoc />
        public int Priority => 200;

        /// <inheritdoc />
        public void Execute(IApplicationBuilder applicationBuilder_, IServiceProvider serviceProvider_)
        {
            applicationBuilder_.UseAuthentication();
        }
    }
}
