using System;
using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace Security.ServiceConfiguration
{
    public class AddAuthorizationPolicies : IConfigureServicesAction
    {
        public int Priority => 3010;

        public void Execute(IServiceCollection services_, IServiceProvider serviceProvider_)
        {
            // TODO later, gather policies provided by extensions

 
        }
    }
}
