// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using Infrastructure.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Policy
{
    /// <summary>
    /// Define a policy with no claims requirements, that will be used when no registered policy is found 
    /// by Infrastructure.Attributes.AuthorizeAttribute.
    /// </summary>
    public class FallbackPolicyProvider
    {
        public const string PolicyName = "Security.FallbackPolicy";

        public AuthorizationPolicy GetAuthorizationPolicy()
        {
            AuthorizationPolicyBuilder authorizationPolicyBuilder = new AuthorizationPolicyBuilder();

            authorizationPolicyBuilder.RequireAssertion(context_ => context_.User.HasClaim(ClaimType.Permission, PolicyName));

            return authorizationPolicyBuilder.Build();
        }
    }
}
