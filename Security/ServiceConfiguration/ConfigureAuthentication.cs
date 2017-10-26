using System;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace Security.ServiceConfiguration
{
    /// <summary>
    /// Configuration of application services to activate authenticated access
    /// </summary>
    public class ConfigureAuthentication : IConfigureServicesAction
    {
        public int Priority => 3000;

        public void Execute(IServiceCollection serviceCollection_, IServiceProvider serviceProvider_)
        {
            serviceCollection_.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options_ =>
                    {
                        options_.AccessDeniedPath = "/account/accessdenied";
                        options_.LoginPath = "/account/signin";
                        options_.LogoutPath = "/account/signout";
                        options_.ReturnUrlParameter = "next";
                        options_.ExpireTimeSpan = TimeSpan.FromDays(7);
                    }
                );
        }
    }
}
