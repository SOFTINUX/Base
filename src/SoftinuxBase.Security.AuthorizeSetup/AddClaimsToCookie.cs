// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.RefreshClaims;
using SoftinuxBase.Security.UserImpersonation.Concrete;

namespace SoftinuxBase.Security.AuthorizeSetup
{
    public static class AddClaimsToCookie
    {
        /// <summary>
        /// This configures how the user's claims get updated with the Permissions/DataKey.
        /// It used the DemoSetupOptions.AuthVersion to control things
        /// </summary>
        /// <param name="services_"></param>
        public static void ConfigureCookiesForExtraAuth(this IServiceCollection services_)
        {
            var sp = services_.BuildServiceProvider();
            var authCookieVersion = sp.GetRequiredService<IOptions<PermissionsSetupOptions>>().Value.AuthVersion;

            IAuthCookieValidate cookieEventClass = null;
            switch (authCookieVersion)
            {
                case AuthCookieVersions.Off:
                    //This turns the permissions/datakey totally off - you are only using ASP.NET Core logged-in user
                    break;
                case AuthCookieVersions.LoginPermissions:
                    //This uses UserClaimsPrincipal to set the claims on login - easy and quick.
                    //Simple version - see https://korzh.com/blogs/net-tricks/aspnet-identity-store-user-data-in-claims
                    services_.AddScoped<IUserClaimsPrincipalFactory<User>, AddPermissionsToUserClaims>();
                    break;
                case AuthCookieVersions.LoginPermissionsDataKey:
                    //This uses UserClaimsPrincipal to set the claims on login - easy and quick.
                    //Simple version - see https://korzh.com/blogs/net-tricks/aspnet-identity-store-user-data-in-claims
                    services_.AddScoped<IUserClaimsPrincipalFactory<User>, AddPermissionsDataKeyToUserClaims>();
                    break;
                case AuthCookieVersions.PermissionsOnly:
                    //Event - only permissions set up
                    cookieEventClass = new AuthCookieValidatePermissionsOnly();
                    break;
                case AuthCookieVersions.PermissionsDataKey:
                     // Event - Permissions and DataKey set up
                     cookieEventClass = new AuthCookieValidatePermissionsDataKey();
                    break;
                case AuthCookieVersions.RefreshClaims:
                    cookieEventClass = new AuthCookieValidateRefreshClaims();
                    break;
                case AuthCookieVersions.Impersonation:
                case AuthCookieVersions.Everything:
                    // Event - Permissions and DataKey set up, provides User Impersonation + possible "RefreshClaims"
                    services_.AddDataProtection();   //DataProtection is needed to encrypt the data in the Impersonation cookie
                    var validateAsyncVersion = authCookieVersion == AuthCookieVersions.Impersonation
                        ? (IAuthCookieValidate)new AuthCookieValidateImpersonation()
                        : new AuthCookieValidateEverything();
                    //We set two events, so we do this here
                    services_.ConfigureApplicationCookie(options =>
                    {
                        options.Events.OnValidatePrincipal = validateAsyncVersion.ValidateAsync;
                        //This ensures the impersonation cookie is deleted when a user signs out
                        options.Events.OnSigningOut = new AuthCookieSigningOut().SigningOutAsync;
                    });
                    break;
                default:
                    throw new ArgumentException($"{authCookieVersion} isn't a valid version");
            }

            if (cookieEventClass != null)
            {
                services_.ConfigureApplicationCookie(options =>
                {
                    options.Events.OnValidatePrincipal = cookieEventClass.ValidateAsync;
                });
            }

            if (authCookieVersion == AuthCookieVersions.RefreshClaims || authCookieVersion == AuthCookieVersions.Everything)
            {
                //IAuthChanges is used to detect changes in the ExtraAuthClasses so we can update the user's permission claims
                services_.AddSingleton<IAuthChanges, AuthChanges>();
            }
            else
            {
                services_.AddSingleton<IAuthChanges>(x => null); //This will turn off the change checks in the ExtraAuthDbContext
            }
        }
    }
}