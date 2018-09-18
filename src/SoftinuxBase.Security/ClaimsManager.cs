// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Security.Claims;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Security.Data.Abstractions;
using Security.Data.Entities;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Common.Enums;
using Permission = SoftinuxBase.Security.Common.Enums.Permission;

namespace SoftinuxBase.Security
{
    internal class ClaimsManager
    {
        private readonly IStorage _storage;
        private readonly UserManager<User> _userManager;

        internal ClaimsManager(IStorage storage_, UserManager<User> userManager_) {
            _storage = storage_;
            _userManager = userManager_;
        }

        /// <summary>
        /// Adds custom claims to WIF-managed ClaimsIdentity object, from application user object.
        /// </summary>
        /// <param name="user_"></param>
        /// <param name="identity_"></param>
        internal async void AddClaims(User user_, ClaimsIdentity identity_)
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

            // Roles
            IList<string> roles = await _userManager.GetRolesAsync(user_);
            foreach (string role in roles)
            {
                identity_.AddClaims(new[] {
                    new Claim(ClaimTypes.Role, role),
                });
            }

            // Permissions
            identity_.AddClaims(GetAllPermissionClaims(user_.Id));

        }

        /// <summary>
        /// Reads all scoped permissions from database and create a custom "permission" claim for every scope, if any permission found for this scope.
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns></returns>
        internal IEnumerable<Claim> GetAllPermissionClaims(string userId_) {
           HashSet<KeyValuePair<Permission, string>> scopedPermissions = _storage.GetRepository<IPermissionRepository>().AllForUser(userId_);
           List<Claim> claims = new List<Claim>();
           Dictionary<string, Permission> permissionByScope = new Dictionary<string, Permission>();
           // Take highest level permission for every scope
           foreach (KeyValuePair<Permission, string> kv in scopedPermissions)
           {
                if(permissionByScope.ContainsKey(kv.Value))
                {
                    if((int) permissionByScope[kv.Value] < (int) kv.Key)
                    {
                        permissionByScope[kv.Value] = kv.Key;
                    }
                }
                else
                {
                    permissionByScope.Add(kv.Value, kv.Key);
                }
           }
            // Now build the claims
           foreach(KeyValuePair<string, Permission> kv in permissionByScope)
           {
               claims.Add(new Claim(ClaimType.Permission, PermissionHelper.GetScopedPermissionIdentifier(kv.Value, kv.Key)));
           }
           return claims;
        }
    }
}