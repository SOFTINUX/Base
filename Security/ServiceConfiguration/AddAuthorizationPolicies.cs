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
        private readonly IStorage _storage;
        public int Priority => 3010;

        public AddAuthorizationPolicies(IStorage storage_)
        {
            _storage = storage_;
        }

        public void Execute(IServiceCollection services_, IServiceProvider serviceProvider_)
        {
            IEnumerable<Permission> permissions = _storage.GetRepository<IPermissionRepository>().All();
            
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
        }
    }
}
