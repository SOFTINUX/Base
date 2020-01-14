// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX.
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Authorization;
using SoftinuxBase.Security.PermissionParts;

namespace SoftinuxBase.Security.FeatureAuthorize.PolicyCode
{
    /// <summary>
    /// Use this attribute onto a controller's class or method to restrict access.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="permissionEnumType_">The enum type</param>
        /// <param name="permission_">Any value of <paramref name="permissionEnumType_"/></param>
        public HasPermissionAttribute(Type permissionEnumType_, short permission_) : base($"{permissionEnumType_.GetAssemblyShortName()}|{permission_.ToString()}")
        {
            
        }

    }
}