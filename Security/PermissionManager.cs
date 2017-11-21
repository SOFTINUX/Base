// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Infrastructure.Interfaces;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.Common;
using Security.Common.Enums;

namespace Security
{
    public class PermissionManager
    {
        /// <summary>
        /// Computes the claims from the permisssions and permission levels.
        /// </summary>
        /// <param name="context_"></param>
        /// <param name="user_"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetFinalPermissions(IRequestHandler context_, User user_)
        {
            return GetFinalPermissions(LoadPermissionLevels(context_, user_),
                context_.Storage.GetRepository<IUserRoleRepository>().FilteredByUserId(user_.Id).FirstOrDefault(ur_ => ur_.RoleId == (int)RoleId.AdministratorOwner) != null);
        }

        /// <summary>
        /// Computes the claims from the permisssions and permission levels.
        /// </summary>
        /// <param name="hasAdminOwnerRole_"></param>
        /// <param name="permissions_"></param>
        /// <returns></returns>
        internal IEnumerable<Claim> GetFinalPermissions(IEnumerable<PermissionValue> permissions_, bool hasAdminOwnerRole_)
        {
            Dictionary<string, PermissionValue> dictPermissionValueByUniqueId =
                new Dictionary<string, PermissionValue>();

            foreach (PermissionValue permission in permissions_)
            {
                if (dictPermissionValueByUniqueId.ContainsKey(permission.UniqueId))
                {
                    dictPermissionValueByUniqueId[permission.UniqueId].Level += permission.Level;
                }
                else
                {
                    dictPermissionValueByUniqueId.Add(permission.UniqueId, permission);
                }
            }

            List<Claim> claims = new List<Claim>();

            foreach (string uniqueId in dictPermissionValueByUniqueId.Keys)
            {
                PermissionValue pv = dictPermissionValueByUniqueId[uniqueId];
                if ((pv.Level % 2 == 1) && !(hasAdminOwnerRole_ && pv.AdministratorOwner))
                {
                    // odd number: "never" flag : no permission.
                    // unless the permission is flagged admin-owner and the user has admin-owner role
                    continue;
                }

                if ((pv.Level & (int)Enums.Permission.PermissionLevelValue.ReadWrite) != 0)
                {
                    // Implicit read write allowed when read-write right allowed
                    claims.Add(new Claim(ClaimType.Permission, PolicyUtil.GetClaimValue(uniqueId, true)));
                    claims.Add(new Claim(ClaimType.Permission, PolicyUtil.GetClaimValue(uniqueId, false)));
                }
                else if ((pv.Level & (int)Enums.Permission.PermissionLevelValue.ReadOnly) != 0)
                {
                    claims.Add(new Claim(ClaimType.Permission, PolicyUtil.GetClaimValue(uniqueId, false)));
                }
            }

            return claims;
        }

        /// <summary>
        /// Loads the permissions from database.
        /// </summary>
        /// <param name="context_"></param>
        /// <param name="user_"></param>
        /// <returns></returns>
        public IEnumerable<PermissionValue> LoadPermissionLevels(IRequestHandler context_, User user_)
        {
            List<PermissionValue> permissions = new List<PermissionValue>();
            IPermissionRepository repo = context_.Storage.GetRepository<IPermissionRepository>();
            // 1. from user's roles
            permissions.AddRange(repo.GetPermissionCodeAndLevelByRoleForUserId(user_.Id));
            // 2. linked to user
            permissions.AddRange(repo.GetPermissionCodeAndLevelByUserId(user_.Id));
            // 3. from user's groups
            permissions.AddRange(repo.GetPermissionCodeAndLevelByGroupForUserId(user_.Id));

            return permissions;
        }
    }
}
