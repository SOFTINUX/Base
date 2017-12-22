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