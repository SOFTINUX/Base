// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SoftinuxBase.Security.DataLayer;
using SoftinuxBase.Security.FeatureAuthorize;

namespace SoftinuxBase.Security.AuthorizeSetup
{
    /// <summary>
    /// This version provides:
    /// - Adds Permissions to the user's claims.
    /// </summary>
    // Thanks to https://korzh.com/blogs/net-tricks/aspnet-identity-store-user-data-in-claims
    public class AddPermissionsToUserClaims : UserClaimsPrincipalFactory<IdentityUser>
    {
        private readonly ApplicationStorageContext _extraAuthDbContext;

        public AddPermissionsToUserClaims(UserManager<IdentityUser> userManager_, IOptions<IdentityOptions> optionsAccessor_,
            ApplicationStorageContext extraAuthDbContext_)
            : base(userManager_, optionsAccessor_)
        {
            _extraAuthDbContext = extraAuthDbContext_;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user_)
        {
            var identity = await base.GenerateClaimsAsync(user_);
            var userId = identity.Claims.GetUserIdFromClaims();
            var rtoPCalcer = new CalcAllowedPermissions(_extraAuthDbContext);
            identity.AddClaim(new Claim(PermissionConstants.PackedPermissionClaimType, await rtoPCalcer.CalcPermissionsForUserAsync(userId)));
            return identity;
        }
    }

}