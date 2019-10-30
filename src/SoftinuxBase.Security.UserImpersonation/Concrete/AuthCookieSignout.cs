// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SoftinuxBase.Security.UserImpersonation.Concrete
{
    public class AuthCookieSigningOut
    {
        /// <summary>
        /// This will ensure any impersonation cookie is deleted when a user signs out
        /// </summary>
        /// <param name="context_"></param>
        /// <returns></returns>
        public Task SigningOutAsync(CookieSigningOutContext context_)
        {
            var cookie = new ImpersonationCookie(context_.HttpContext, null);
            cookie.Delete();

            return Task.CompletedTask;
        }
    }
}