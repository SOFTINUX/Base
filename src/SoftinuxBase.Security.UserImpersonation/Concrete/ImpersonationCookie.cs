// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

// TODO CHANGE ASSEMBLY TO SECURITY UNIT TEST
[assembly: InternalsVisibleTo("Test")]

namespace SoftinuxBase.Security.UserImpersonation.Concrete
{
    public class ImpersonationCookie
    {
        private const string CookieName = "UserImpersonation";
        private readonly HttpContext _httpContext;
        private readonly IDataProtectionProvider _protectionProvider;
        private readonly CookieOptions _options;
        private readonly ILogger _logger;
        public string EncryptPurpose { get; private set; }

        public ImpersonationCookie(HttpContext httpContext_, IDataProtectionProvider protectionProvider_)
        {
            _httpContext = httpContext_ ?? throw new ArgumentNullException(nameof(httpContext_));
            _protectionProvider = protectionProvider_; //Can be null
            EncryptPurpose = "hffhegse432!&2!jbK!K3wqqqagg3bbassdewdsgfedgbfdewe13c";
            _options = new CookieOptions
            {
                Secure = false,  //In real life you would want this to be true, but for this demo I allow http
                HttpOnly = true, //Not used by JavaScript
                IsEssential = true,
                //These two make it a session cookie, i.e. it disappears when the browser is closed
                Expires = null,
                MaxAge = null
            };
        }

        public void AddUpdateCookie(string data_)
        {
            if (_protectionProvider == null)
                throw new NullReferenceException(
                    $"The {nameof(IDataProtectionProvider)} was null, which means impersonation is turned off.");

            var protector = _protectionProvider.CreateProtector(EncryptPurpose);
            var encryptedString = protector.Protect(data_);
            _httpContext.Response.Cookies.Append(CookieName, encryptedString, _options);
        }

        public bool Exists(IRequestCookieCollection cookiesIn_)
        {
            return cookiesIn_[CookieName] != null;
        }

        public string GetCookieInValue()
        {
            if (_protectionProvider == null)
                throw new NullReferenceException(
                    $"The {nameof(IDataProtectionProvider)} was null, which means impersonation is turned off.");

            var cookieData = _httpContext.Request.Cookies[CookieName];
            if (string.IsNullOrEmpty(cookieData))
                return null;

            var protector = _protectionProvider.CreateProtector(EncryptPurpose);
            string decrypt;
            try
            {
                decrypt = protector.Unprotect(cookieData);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error decoding a cookie. Have deleted cookie to stop any further problems.");
                Delete();
                throw;
            }

            return decrypt;
        }

        public void Delete()
        {
            _httpContext.Response.Cookies.Delete(CookieName, _options);
        }
    }
}