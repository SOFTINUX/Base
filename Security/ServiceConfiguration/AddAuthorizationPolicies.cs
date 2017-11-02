using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.ServiceConfiguration
{
    public class AddAuthorizationPolicies : IConfigureServicesAction
    {
        /// <summary>
        /// Executes after ActivateAuthentication and ConfigureAuthentication service actions.
        /// </summary>
        public int Priority => 201;

        /// <summary>
        /// Necessary public empty constructor.
        /// </summary>
        public AddAuthorizationPolicies() { }

        public void Execute(IServiceCollection services_, IServiceProvider serviceProvider_)
        {
        /*    IStorage storage = serviceProvider_.GetService<IStorage>();
            //IStorage storage = services_.BuildServiceProvider().GetService<IStorage>();
            // Doesn't work, it cannot find a IStorageContext implementation in services_...
            IEnumerable<Permission> permissions = storage.GetRepository<IPermissionRepository>().All();

            services_.AddAuthorization(options_ =>
                {
                    foreach (Permission permission in permissions)
                    {
                        string claimValue = PermissionManager.GetClaimValue(permission.UniqueIdentifier, false);
                            options_.AddPolicy(claimValue, policy_ => {
                                policy_.RequireClaim(claimValue);
                            });
                        claimValue = PermissionManager.GetClaimValue(permission.UniqueIdentifier, true);
                        options_.AddPolicy(claimValue, policy => {
                            policy.RequireClaim(claimValue);
                        });
                    }

                }
            );
            */
        }
    }
}
