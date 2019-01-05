// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Mvc.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace SoftinuxBase.Barebone.Actions
{
    public class UseMvcActions : IUseMvcAction
    {
        public int Priority => 100;

        public void Execute(IRouteBuilder routBuilder_, IServiceProvider serviceProvider_)
        {
            routBuilder_.MapRoute("Default", "{controller}/{action}", new { controller = "Barebone", action = "Index" });
        }
    }
}