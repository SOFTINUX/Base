using System;
using System.Collections.Generic;
using System.Security.Claims;
using Infrastructure;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security
{
    public class PermissionManager : IPermissionManager
    {
        /// <summary>
        /// Computes the claims from the permisssions and permission levels.
        /// </summary>
        /// <param name="context_"></param>
        /// <param name="user_"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetFinalPermissions(IRequestHandler context_, User user_)
        {
            return GetFinalPermissions(LoadPermissionLevels(context_, user_));
        }

        /// <summary>
        /// Computes the claims from the permisssions and permission levels.
        /// </summary>
        /// <param name="permissions_"></param>
        /// <returns></returns>
        internal IEnumerable<Claim> GetFinalPermissions(IEnumerable<Tuple<string, int>> permissions_)
        {
            Dictionary<string, int> dictUniqueIdAndLevel = new Dictionary<string, int>();

            foreach (Tuple<string, int> permission in permissions_)
            {
                if (dictUniqueIdAndLevel.ContainsKey(permission.Item1))
                {
                    dictUniqueIdAndLevel[permission.Item1] += permission.Item2;
                }
                else
                {
                    dictUniqueIdAndLevel.Add(permission.Item1, permission.Item2);
                }
            }

            List<Claim> claims = new List<Claim>();

            foreach (string uniqueId in dictUniqueIdAndLevel.Keys)
            {
                if (dictUniqueIdAndLevel[uniqueId] % 2 == 1)
                {
                    // odd number: "never" flag : no permission
                    continue;
                }

                if ((dictUniqueIdAndLevel[uniqueId] & (int) Enums.Permission.PermissionLevelValue.ReadWrite) != 0)
                {
                    claims.Add(new Claim(Enums.ClaimType.Permission, uniqueId + "|RW"));
                } else if ((dictUniqueIdAndLevel[uniqueId] & (int)Enums.Permission.PermissionLevelValue.ReadOnly) != 0)
                {
                    claims.Add(new Claim(Enums.ClaimType.Permission, uniqueId + "|RO"));
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
        public IEnumerable<Tuple<string, int>> LoadPermissionLevels(IRequestHandler context_, User user_)
        {
            List<Tuple<string, int>> permissions = new List<Tuple<string, int>>();
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
