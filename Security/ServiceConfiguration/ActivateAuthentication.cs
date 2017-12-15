// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;

namespace Security.ServiceConfiguration
{
    /// <summary>
    /// Activates the authentication.
    /// </summary>
    public class ActivateAuthentication : IConfigureAction
    {
        public int Priority => 201;

        public void Execute(IApplicationBuilder applicationBuilder_, IServiceProvider serviceProvider_)
        {
            applicationBuilder_.UseAuthentication();
        }
    }
}
