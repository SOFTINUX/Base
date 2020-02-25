// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Security.Permissions.Filters;

namespace SoftinuxBase.Security.Permissions.Attributes
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimRequirementAttribute"/> class.
        /// </summary>
        /// <param name="claimType_">Type of claim.</param>
        /// <param name="claimValue_">Value of claim.</param>
        public ClaimRequirementAttribute(string claimType_, string claimValue_) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType_, claimValue_) };
        }
    }
}
