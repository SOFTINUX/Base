﻿// Copyright © 2017-2019 SOFTINUX. All rights reserved.
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
        /// <inheritdoc />
        public int Priority => 200;

        /// <inheritdoc />
        public void Execute(IApplicationBuilder applicationBuilder_, IServiceProvider serviceProvider_)
        {
            applicationBuilder_.UseAuthentication();
        }
    }
}
