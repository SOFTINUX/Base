// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Policy;
using Microsoft.Extensions.DependencyInjection;
using Security.Data.Abstractions;

namespace Security
{
    public static class PoliciesManager
    {
        /// <summary>
        /// Defines the available policies known by the application.
        /// To be called at application startup and after any change to Permission records.
        /// </summary>
        /// <param name="services_"></param>
        /// <param name="storage_"></param>
        public static void DefineAvailablePolicies(IServiceCollection services_, IStorage storage_)
        {
            System.Collections.Generic.IEnumerable<Security.Data.Entities.Permission> permissions =
                storage_.GetRepository<IPermissionRepository>().All();

            services_.AddAuthorization(options_ =>
            {
                foreach (Security.Data.Entities.Permission permission in permissions)
                {
                    string claimValueAndPolicyName = PolicyUtil.GetClaimValue(permission.UniqueIdentifier);
                    options_.AddPolicy(claimValueAndPolicyName,
                        policy_ => { policy_.RequireClaim(ClaimType.Permission, claimValueAndPolicyName); });

                    KnownPolicies.Add(claimValueAndPolicyName);
                }

                // and the fallback policy
                FallbackPolicyProvider fallbackPolicyProvider = new FallbackPolicyProvider();
                options_.AddPolicy(FallbackPolicyProvider.PolicyName, fallbackPolicyProvider.GetAuthorizationPolicy());
                KnownPolicies.Add(FallbackPolicyProvider.PolicyName);
            });
        }
    }
}