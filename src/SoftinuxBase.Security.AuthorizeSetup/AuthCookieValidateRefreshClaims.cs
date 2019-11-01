// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using SoftinuxBase.Security.DataAuthorize;
using SoftinuxBase.Security.DataKeyParts;
using SoftinuxBase.Security.DataLayer;
using SoftinuxBase.Security.FeatureAuthorize;
using SoftinuxBase.Security.RefreshClaimsParts;

namespace SoftinuxBase.Security.AuthorizeSetup
{
    /// <summary>
    /// This version provides:
    /// - Adds Permissions to the user's claims.
    /// - Adds DataKey to the user's claims
    /// - AND the user's claims are updated if there is a change in the roles/datakey information
    /// </summary>
    public class AuthCookieValidateRefreshClaims : IAuthCookieValidate
    {
        public async Task ValidateAsync(CookieValidatePrincipalContext context_)
        {
            var authChanges = new AuthChanges();
            var extraContext = context_.HttpContext.RequestServices.GetRequiredService<ApplicationStorageContext>();

            var newClaims = new List<Claim>();
            var originalClaims = context_.Principal.Claims.ToList();
            if (originalClaims.All(x => x.Type != PermissionConstants.PackedPermissionClaimType) ||
                authChanges.IsOutOfDateOrMissing(AuthChangesConsts.FeatureCacheKey,
                    originalClaims.SingleOrDefault(x => x.Type == PermissionConstants.LastPermissionsUpdatedClaimType)?.Value,
                    extraContext))
            {
                var rtoPCalcer = new CalcAllowedPermissions(extraContext);
                var dataKeyCalc = new CalcDataKey(extraContext);

                //Handle the feature permissions
                var userId = originalClaims.GetUserIdFromClaims();
                newClaims.AddRange(await BuildFeatureClaimsAsync(userId, rtoPCalcer));
                newClaims.AddRange(BuildDataClaims(userId, dataKeyCalc));

                //Something has changed so we replace the current ClaimsPrincipal with a new one

                newClaims.AddRange(RemoveUpdatedClaimsFromOriginalClaims(originalClaims, newClaims)); //Copy over unchanged claims
                //Build a new ClaimsPrincipal and use it to replace the current ClaimsPrincipal
                var identity = new ClaimsIdentity(newClaims, "Cookie");
                var newPrincipal = new ClaimsPrincipal(identity);
                context_.ReplacePrincipal(newPrincipal);
                //THIS IS IMPORTANT: This updates the cookie, otherwise this calc will be done every HTTP request
                context_.ShouldRenew = true;
            }
        }

        private IEnumerable<Claim> RemoveUpdatedClaimsFromOriginalClaims(List<Claim> originalClaims_, List<Claim> newClaims_)
        {
            var newClaimTypes = newClaims_.Select(x => x.Type);
            return originalClaims_.Where(x => !newClaimTypes.Contains(x.Type));
        }

        private async Task<List<Claim>> BuildFeatureClaimsAsync(string userId_, CalcAllowedPermissions rtoP_)
        {
            var claims = new List<Claim>
            {
                new Claim(PermissionConstants.PackedPermissionClaimType, await rtoP_.CalcPermissionsForUserAsync(userId_)),
                new Claim(PermissionConstants.LastPermissionsUpdatedClaimType, DateTime.UtcNow.Ticks.ToString())
            };
            return claims;
        }

        private List<Claim> BuildDataClaims(string userId_, CalcDataKey dataKeyCalc_)
        {
            var claims = new List<Claim>
            {
                new Claim(DataAuthConstants.HierarchicalKeyClaimName, dataKeyCalc_.CalcDataKeyForUser(userId_))
            };
            return claims;
        }
    }
}