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
using SoftinuxBase.Security.Data.EntityFramework;
using SoftinuxBase.Security.FeatureAuthorize;
using SoftinuxBase.Security.RefreshClaimsParts;
using SoftinuxBase.Security.UserImpersonation.Concrete;

namespace SoftinuxBase.Security.AuthorizeSetup
{
    /// <summary>
    /// This version provides:
    /// - Adds Permissions to the user's claims.
    /// - Adds DataKey to the user's claims
    /// - AND allows for user impersonation
    /// - AND the user's claims are updated if there is a change in the roles/datakey information
    /// </summary>
    public class AuthCookieValidateEverything : IAuthCookieValidate
    {
        /// <summary>
        /// This will set up the user's feature permissions if either of the following states are found
        /// - The current claims doesn't have the PackedPermissionClaimType. This happens when someone logs in.
        /// - If the LastPermissionsUpdatedClaimType is missing (null) or is a lower number that is stored in the TimeStore cache.
        /// </summary>
        /// <param name="context_"></param>
        /// <returns></returns>
        public async Task ValidateAsync(CookieValidatePrincipalContext context_)
        {
            var extraContext = context_.HttpContext.RequestServices.GetRequiredService<ApplicationStorageContext>();
            var protectionProvider = context_.HttpContext.RequestServices.GetService<IDataProtectionProvider>();
            var authChanges = new AuthChanges();

            var originalClaims = context_.Principal.Claims.ToList();
            var impHandler = new ImpersonationHandler(context_.HttpContext, protectionProvider, originalClaims);

            var newClaims = new List<Claim>();
            if (originalClaims.All(x => x.Type != PermissionConstants.PackedPermissionClaimType) ||
                impHandler.ImpersonationChange ||
                authChanges.IsOutOfDateOrMissing(AuthChangesConsts.FeatureCacheKey,
                    originalClaims.SingleOrDefault(x => x.Type == PermissionConstants.LastPermissionsUpdatedClaimType)?.Value,
                    extraContext))
            {
                var rtoPCalcer = new CalcAllowedPermissions(extraContext);

                //Handle the feature permissions
                var permissionUserId = impHandler.GetUserIdForWorkingOutPermissions();
                newClaims.AddRange(await BuildFeatureClaimsAsync(permissionUserId, rtoPCalcer));

                //Something has changed so we replace the current ClaimsPrincipal with a new one

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