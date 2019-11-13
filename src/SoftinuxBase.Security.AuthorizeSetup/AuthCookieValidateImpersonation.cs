// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using SoftinuxBase.Security.DataKeyParts;
using SoftinuxBase.Security.DataLayer;
using SoftinuxBase.Security.FeatureAuthorize;
using SoftinuxBase.Security.UserImpersonation.Concrete;

namespace SoftinuxBase.Security.AuthorizeSetup
{
    /// <summary>
    /// This version provides:
    /// - Adds Permissions to the user's claims.
    /// - Adds DataKey to the user's claims
    /// - AND user impersonation
    /// </summary>
    public class AuthCookieValidateImpersonation : IAuthCookieValidate
    {
        public async Task ValidateAsync(CookieValidatePrincipalContext context_)
        {
            var originalClaims = context_.Principal.Claims.ToList();
            var protectionProvider = context_.HttpContext.RequestServices.GetService<IDataProtectionProvider>();
            var impHandler = new ImpersonationHandler(context_.HttpContext, protectionProvider, originalClaims);

            var newClaims = new List<Claim>();
            if (originalClaims.All(x => x.Type != PermissionConstants.PackedPermissionClaimType) ||
                impHandler.ImpersonationChange)
            {
                //There is no PackedPermissionClaimType or there was a change in the impersonation state

                var extraContext = context_.HttpContext.RequestServices.GetRequiredService<ApplicationStorageContext>();
                var rtoPCalcer = new CalcAllowedPermissions(extraContext);

                var userId = impHandler.GetUserIdForWorkingDataKey();
                newClaims.AddRange(await BuildFeatureClaimsAsync(userId, rtoPCalcer));

                newClaims.AddRange(RemoveUpdatedClaimsFromOriginalClaims(originalClaims, newClaims)); //Copy over unchanged claims
                impHandler.AddOrRemoveImpersonationClaim(newClaims);
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

    }
}