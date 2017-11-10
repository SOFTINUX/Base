// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Security.Enums;

namespace Security.Policy
{
    /// <summary>
    /// Define a policy with no claims requirements, that will be used when no registered policy is found by Security.AuthorizeAttribute.
    /// </summary>
    public class FallbackPolicyProvider
    {
        public static string PolicyName => "Infrastructure.FallbackPolicy";

        public AuthorizationPolicy GetAuthorizationPolicy()
        {
            AuthorizationPolicyBuilder authorizationPolicyBuilder = new AuthorizationPolicyBuilder();

            authorizationPolicyBuilder.RequireAssertion(context_ => context_.User.HasClaim(ClaimType.Permission, PolicyName));

            return authorizationPolicyBuilder.Build();
        }
    }
}
