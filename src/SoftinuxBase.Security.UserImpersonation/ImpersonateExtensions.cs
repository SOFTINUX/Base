// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Html;
using SoftinuxBase.Security.UserImpersonation.Concrete;

namespace SoftinuxBase.Security.UserImpersonation
{
    public static class ImpersonateExtensions
    {
        public static bool InImpersonationMode(this ClaimsPrincipal claimsPrincipal_)
        {
            return claimsPrincipal_.Claims.Any(x => x.Type == ImpersonationHandler.ImpersonationClaimType);
        }

        public static string GetImpersonatedUserNameMode(this ClaimsPrincipal claimsPrincipal_)
        {
            return claimsPrincipal_.Claims.SingleOrDefault(x => x.Type == ImpersonationHandler.ImpersonationClaimType)?.Value;
        }

        public static HtmlString GetCurrentUserNameAsHtml(this ClaimsPrincipal claimsPrincipal_)
        {
            var impersonalisedName = claimsPrincipal_.GetImpersonatedUserNameMode();
            var nameToShow = impersonalisedName ??
                             claimsPrincipal_.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value ??
                             "not logged in";

            return new HtmlString(
                "<span" + (impersonalisedName != null ? " class=\"text-danger\">Impersonating " : ">Hello ")
                     + $"{nameToShow}</span>");
        }
    }
}