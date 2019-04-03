// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Security.Common.Filters;

namespace SoftinuxBase.Security.Common.Attributes
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimRequirementAttribute"/> class.
        /// </summary>
        /// <param name="claimType_"></param>
        /// <param name="claimValue_"></param>
        public ClaimRequirementAttribute(string claimType_, string claimValue_) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType_, claimValue_) };
        }
    }
}
