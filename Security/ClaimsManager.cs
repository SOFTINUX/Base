// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Security.Claims;
using ExtCore.Data.Abstractions;
using Security.Common.Enums;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security
{
    public class ClaimsManager
    {
        private IStorage _storage;

        public ClaimsManager(IStorage storage_) {
            _storage = storage_;
        }

        /// <summary>
        /// Adds custom claims to WIF-managed ClaimsIdentity object, from application user object.
        /// </summary>
        /// <param name="user_"></param>
        /// <param name="identity_"></param>
        public void AddClaims(User user_, ClaimsIdentity identity_)
        {
            // First name
            if (!string.IsNullOrWhiteSpace(user_.FirstName))
            {
                identity_.AddClaims(new[] {
                    new Claim(ClaimTypes.GivenName, user_.FirstName)
                });
            }

            // Last name
            if (!string.IsNullOrWhiteSpace(user_.LastName))
            {
                identity_.AddClaims(new[] {
                    new Claim(ClaimTypes.Surname, user_.LastName),
                });
            }

            // Permissions
            identity_.AddClaims(new ClaimsManager(_storage).GetAllPermissionClaims(user_.Id));

        }

        /// <summary>
        /// Reads all permissions from database and create a custom "permission" claim for each.
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetAllPermissionClaims(string userId_) {
           IEnumerable<Permission> permissions = _storage.GetRepository<IPermissionRepository>().AllForUser(userId_);
           List<Claim> claims = new List<Claim>();
           foreach (Permission p in permissions)
           {
               claims.Add(new Claim(ClaimType.Permission, p.UniqueIdentifier));
           }
           return claims;
        }
    }
}