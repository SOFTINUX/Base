using System;
using ExtCore.Infrastructure;
using ExtCore.Infrastructure.Actions;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Security.ServiceConfiguration
{
    public class AddAuthorizationPolicies : IConfigureServicesAction
    {
        public int Priority => 3010;

        public void Execute(IServiceCollection services_, IServiceProvider serviceProvider_)
        {
            services_.AddAuthorization(options =>
                {
                    foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
                    {
                        if (extensionMetadata.AuthorizationPolicyProviders == null) continue;
                        foreach (IAuthorizationPolicyProvider authorizationPolicyProvider in extensionMetadata
                            .AuthorizationPolicyProviders)
                        {
                            var policy = authorizationPolicyProvider.GetDefaultPolicyAsync().Result;
                            options.AddPolicy(policy.ToString(), policy);
                        }
                            
                    }
                }
            );
        }
    }
}

// Reread code here: https://www.bbsmax.com/A/q4zVO6rXJK/ and ms docs
