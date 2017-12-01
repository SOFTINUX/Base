// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Security.Claims;
using Security.Common.Policy;

namespace Security.Common.Attributes
{
    public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        /// <summary>
        /// Safe use of Microsoft.AspNetCore.Authorization.AuthorizeAttribute: this version won't crash your application
        /// if you try to use a non-existent policy. It checks that it exists before calling Microsoft.AspNetCore.Authorization.AuthorizeAttribute
        /// with policy name as parameter.
        /// Note: the parameters are converted to lower case.
        /// </summary>
        /// <param name="permissionCode_">Code of permission, unique among the permissions defined by one extension.</param>
        /// <param name="extensionAssemblySimpleName_">Extension assembly simple name, typically given by Assembly.GetAssemblyName().Name</param>
        /// <param name="accessLevel_">Access level, read-only or read-write</param>
        public AuthorizeAttribute(string permissionCode_, string extensionAssemblySimpleName_, PolicyUtil.AccessLevel accessLevel_)
        {
            string policyName = PolicyUtil.GetPolicyName(permissionCode_, extensionAssemblySimpleName_, accessLevel_).ToLowerInvariant();

            // We need to check that the corresponding permission exists
            Policy = KnownPolicies.Contains(policyName) ? policyName : FallbackPolicyProvider.PolicyName;

            // Debug - le problème est ici, comprendre ce qui a été mal fait pour que ClaimsPrincipal.Current soit null
            // on n'a pas bien rattaché le user et ses claims au contexte applicatif d'identité (ClaimsPrincipal.Current).
            // Le code concerné est dans UserManager.LoadClaims()

            var currentUser = ClaimsPrincipal.Current;
        }

    }
}
