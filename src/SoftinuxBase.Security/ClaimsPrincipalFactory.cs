﻿// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Security.Claims;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security
{
    /// <inheritdoc />
    /// <summary>
    /// Overriding WIF's UserClaimsPrincipalFactory allows to add custom claims to the WIF's current user.
    /// </summary>
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole<string>>
    {
        private readonly IStorage _storage;

        public ClaimsPrincipalFactory(UserManager<User> userManager_, RoleManager<IdentityRole<string>> roleManager_, IOptions<IdentityOptions> optionsAccessor_, IStorage storage_)
            : base(userManager_, roleManager_, optionsAccessor_)
        {
            _storage = storage_;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(User user_)
        {
            var principal = await base.CreateAsync(user_);

            await new ClaimsManager(_storage, UserManager).AddClaimsAsync(user_, (ClaimsIdentity)principal.Identity);

            return principal;
        }
    }
}
