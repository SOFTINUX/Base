using System.Collections.Generic;
using System.Security.Claims;
using Infrastructure;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.Data.EntityFramework;

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
        internal IEnumerable<Claim> GetFinalPermissions(IEnumerable<PermissionValue> permissions_)
        {
            Dictionary<string, int> dictUniqueIdAndLevel = new Dictionary<string, int>();

            foreach (PermissionValue permission in permissions_)
            {
                if (dictUniqueIdAndLevel.ContainsKey(permission.UniqueId))
                {
                    dictUniqueIdAndLevel[permission.UniqueId] += permission.Level;
                }
                else
                {
                    dictUniqueIdAndLevel.Add(permission.UniqueId, permission.Level);
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
