// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using SoftinuxBase.Security.FeatureAuthorize;

namespace SoftinuxBase.Security.UserImpersonation.Concrete
{
    public class ImpersonationService : IImpersonationService
    {
        private readonly HttpContext _httpContext;
        private readonly ImpersonationCookie _cookie;

        public ImpersonationService(IHttpContextAccessor httpContextAccessor_, IDataProtectionProvider protectionProvider_)
        {
            _httpContext = httpContextAccessor_.HttpContext;
            _cookie = protectionProvider_ != null //If protectionProvider is null then impersonation is turned off
                    ? new ImpersonationCookie(_httpContext, protectionProvider_)
                    : null;
        }

        /// <summary>
        /// This creates an user impersonation cookie, which starts the user impersonation via the AuthCookie ValidateAsync event
        /// </summary>
        /// <param name="userId_">This must be the userId of the user you want to impersonate</param>
        /// <param name="userName_"></param>
        /// <param name="keepOwnPermissions_"></param>
        /// <returns>Error message, or null if OK.</returns>
        public string StartImpersonation(string userId_, string userName_, bool keepOwnPermissions_)
        {
            if (_cookie == null)
                return "Impersonation is turned off in this application.";
            if (!_httpContext.User.Identity.IsAuthenticated)
                return "You must be logged in to impersonate a user.";
            if (_httpContext.User.Claims.GetUserIdFromClaims() == userId_)
                return "You cannot impersonate yourself.";
            if (_httpContext.User.InImpersonationMode())
                return "You are already in impersonation mode.";
            if (userId_ == null)
                return "You must provide a userId string";
            if (userName_ == null)
                return "You must provide a username string";

            _cookie.AddUpdateCookie(new ImpersonationData(userId_, userName_, keepOwnPermissions_).GetPackImpersonationData());
            return null;
        }

        /// <summary>
        /// This will delete the user impersonation cookie, which causes the AuthCookie ValidateAsync event to revert to the original user
        /// </summary>
        /// <returns>error message, or null if OK</returns>
        public string StopImpersonation()
        {
            if (!_httpContext.User.InImpersonationMode())
                return "You aren't in impersonation mode.";

            _cookie.Delete();
            return null;
        }
    }
}