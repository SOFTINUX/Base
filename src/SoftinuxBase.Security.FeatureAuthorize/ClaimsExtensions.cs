// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SoftinuxBase.Security.FeatureAuthorize
{
    public static class ClaimsExtensions
    {
        public static string GetUserIdFromClaims(this IEnumerable<Claim> claims_)
        {
            return claims_?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}