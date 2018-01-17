// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

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
        public AuthorizeAttribute(string permissionCode_, string extensionAssemblySimpleName_)
        {
            string policyName = PolicyUtil.GetPolicyName(permissionCode_, extensionAssemblySimpleName_);

            // We need to check that the corresponding permission exists
            Policy = KnownPolicies.Contains(policyName) ? policyName : FallbackPolicyProvider.PolicyName;
        }

    }
}
