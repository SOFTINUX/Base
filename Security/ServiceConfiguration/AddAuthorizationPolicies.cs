// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.Common;
using Security.Common.Policy;

namespace Security.ServiceConfiguration
{
    public class AddAuthorizationPolicies : IConfigureServicesAction
    {
        /// <summary>
        /// Executes after ActivateAuthentication and ConfigureAuthentication service actions.
        /// </summary>
        public int Priority => 201;

        /// <summary>
        /// Necessary public empty constructor.
        /// </summary>
        public AddAuthorizationPolicies() { }

        public void Execute(IServiceCollection services_, IServiceProvider serviceProvider_)
        {
            IStorage storage = serviceProvider_.GetService<IStorage>();

            IEnumerable<Permission> permissions = storage.GetRepository<IPermissionRepository>().All();

            services_.AddAuthorization(options_ =>
                {
                    foreach (Permission permission in permissions)
                    {
                        string claimValueAndPolicyName = PolicyUtil.GetClaimValue(permission.UniqueIdentifier, false);
                        options_.AddPolicy(claimValueAndPolicyName, policy_ =>
                        {
                            policy_.RequireClaim(claimValueAndPolicyName);
                        });
                        claimValueAndPolicyName = PolicyUtil.GetClaimValue(permission.UniqueIdentifier, true);
                        options_.AddPolicy(claimValueAndPolicyName, policy =>
                        {
                            policy.RequireClaim(claimValueAndPolicyName);
                        });

                        KnownPolicies.Add(claimValueAndPolicyName);
                    }

                    // and the fallback policy
                    FallbackPolicyProvider fallbackPolicyProvider = new FallbackPolicyProvider();
                    options_.AddPolicy(FallbackPolicyProvider.PolicyName, fallbackPolicyProvider.GetAuthorizationPolicy());
                    KnownPolicies.Add(FallbackPolicyProvider.PolicyName);

                }
            );

        }
    }
}
