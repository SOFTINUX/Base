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
        public int Priority => 200;

        public void Execute(IApplicationBuilder applicationBuilder_, IServiceProvider serviceProvider_)
        {
            applicationBuilder_.UseAuthentication();
        }
    }
}
