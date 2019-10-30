// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using SoftinuxBase.Security.DataAuthorize;
using SoftinuxBase.Security.DataKeyParts;
using SoftinuxBase.Security.DataLayer.EfCode;
using SoftinuxBase.Security.FeatureAuthorize;

namespace SoftinuxBase.Security.AuthorizeSetup
{
    /// <summary>
    /// This version provides:
    /// - Adds Permissions to the user's claims.
    /// - Adds DataKey to the user's claims
    /// </summary>
    public class AuthCookieValidatePermissionsDataKey : IAuthCookieValidate
    {
        public async Task ValidateAsync(CookieValidatePrincipalContext context_)
        {
            if (context_.Principal.Claims.Any(x => x.Type == PermissionConstants.PackedPermissionClaimType))
                return;

            //No permissions in the claims, so we need to add it. This is only happen once after the user has logged in
            var extraContext = context_.HttpContext.RequestServices.GetRequiredService<ExtraAuthorizeDbContext>();
            var rtoPCalcer = new CalcAllowedPermissions(extraContext);
            var dataKeyCalc = new CalcDataKey(extraContext);

            var claims = new List<Claim>();
            claims.AddRange(context_.Principal.Claims); //Copy over existing claims
            var userId = context_.Principal.Claims.GetUserIdFromClaims();
            //Now calculate the Permissions Claim value and add it
            claims.Add(new Claim(PermissionConstants.PackedPermissionClaimType,
                await rtoPCalcer.CalcPermissionsForUserAsync(userId)));
            //and the same for the DataKey
            claims.Add(new Claim(DataAuthConstants.HierarchicalKeyClaimName,
                dataKeyCalc.CalcDataKeyForUser(userId)));

            //Build a new ClaimsPrincipal and use it to replace the current ClaimsPrincipal
            var identity = new ClaimsIdentity(claims, "Cookie");
            var newPrincipal = new ClaimsPrincipal(identity);
            context_.ReplacePrincipal(newPrincipal);
            //THIS IS IMPORTANT: This updates the cookie, otherwise this calc will be done every HTTP request
            context_.ShouldRenew = true;
        }
    }
}