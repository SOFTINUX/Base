// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Security.Claims;
using ExtCore.Data.Abstractions;
using Infrastructure;
using Infrastructure.Enums;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security
{
    internal class ClaimsManager
    {
        private IStorage _storage;

        internal ClaimsManager(IStorage storage_) {
            _storage = storage_;
        }

        /// <summary>
        /// Adds custom claims to WIF-managed ClaimsIdentity object, from application user object.
        /// </summary>
        /// <param name="user_"></param>
        /// <param name="identity_"></param>
        internal void AddClaims(User user_, ClaimsIdentity identity_)
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
        /// Reads all scoped permissions from database and create a custom "permission" claim for each.
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns></returns>
        internal IEnumerable<Claim> GetAllPermissionClaims(string userId_) {
            HashSet<KeyValuePair<Infrastructure.Enums.Permission, string>> scopedPermissions = _storage.GetRepository<IPermissionRepository>().AllForUser(userId_);
           List<Claim> claims = new List<Claim>();
           foreach (KeyValuePair<Infrastructure.Enums.Permission, string> kv in scopedPermissions)
           {
               claims.Add(new Claim(ClaimType.Permission, Util.GetScopedPermissionIdentifier(kv.Key, kv.Value)));
           }
           return claims;
        }
    }
}