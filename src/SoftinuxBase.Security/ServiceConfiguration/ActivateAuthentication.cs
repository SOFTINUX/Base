// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;

namespace SoftinuxBase.Security.ServiceConfiguration
{
    /// <summary>
    /// Activates the authentication.
    /// </summary>
    public class ActivateAuthentication : IConfigureAction
    {
        public int Priority => 200;

        /// <summary>
        /// TODO DOCUMENT ME.
        /// </summary>
        /// <param name="applicationBuilder_"></param>
        /// <param name="serviceProvider_"></param>
        public void Execute(IApplicationBuilder applicationBuilder_, IServiceProvider serviceProvider_)
        {
            applicationBuilder_.UseAuthentication();
        }
    }
}
