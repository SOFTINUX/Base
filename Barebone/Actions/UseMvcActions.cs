using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using ExtCore.Mvc.Infrastructure.Actions;

namespace Barebone.Actions
{
    public class UseMvcActions : IUseMvcAction
    {
        public int Priority => 100;

        public void Execute(IRouteBuilder routBuilder_, IServiceProvider serviceProvider_)
        {
            routBuilder_.MapRoute(name: "Default", template: "{controller}/{action}", defaults: new { controller = "Barebone", action = "Index"});
        }
    }
}