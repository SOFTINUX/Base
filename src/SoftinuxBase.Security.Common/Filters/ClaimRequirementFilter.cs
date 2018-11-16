// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SoftinuxBase.Security.Common.Filters
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim_)
        {
            _claim = claim_;
        }

        public void OnAuthorization(AuthorizationFilterContext context_)
        {
            var hasClaim = context_.HttpContext.User.Claims.Any(c_ => c_.Type == _claim.Type && c_.Value == _claim.Value);
            if (!hasClaim)
            {
                context_.Result = new ForbidResult();
            }
        }
    }
}