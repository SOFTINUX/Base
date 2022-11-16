// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;

namespace SoftinuxBase.Barebone.Actions
{
    public class UseAuthorizationAction : IConfigureAction
    {
        /// <inheritdoc />
        public int Priority => 10001;

       /// <inheritdoc />
        public void Execute(IApplicationBuilder applicationBuilder_, IServiceProvider serviceProvider_)
        {
            applicationBuilder_.UseAuthorization();
        }
    }
}