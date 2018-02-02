// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Security.Claims;
using Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Attributes
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType_, string claimValue_) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType_, claimValue_) };
        }
    }
}
