// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure.Actions;
using Infrastructure;
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
            IStorage storage = serviceProvider_.GetService<IStorage>();

            IEnumerable<Permission> permissions = storage.GetRepository<IPermissionRepository>().All();

            services_.AddAuthorization(options_ =>
                {
                    foreach (Permission permission in permissions)
                    {
                        string claimValue = PolicyUtil.GetClaimValue(permission.UniqueIdentifier, false);
                            options_.AddPolicy(claimValue, policy_ => {
                                policy_.RequireClaim(claimValue);
                            });
                        claimValue = PolicyUtil.GetClaimValue(permission.UniqueIdentifier, true);
                        options_.AddPolicy(claimValue, policy => {
                            policy.RequireClaim(claimValue);
                        });
                    }

                }
            );

        }
    }
}
