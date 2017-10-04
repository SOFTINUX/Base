using System.Collections.Generic;
using System.Security.Claims;
using Infrastructure;
using Security.Data.Entities;

namespace Security
{
    public class PermissionManager : IPermissionManager
    {

        /// <summary>
        /// Computes the claims from the permisssions and permission levels.
        /// </summary>
        /// <param name="user_"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetFinalPermissions(User user_)
        {
            return GetFinalPermissions(LoadPermissions(user_));
        }

        /// <summary>
        /// Computes the claims from the permisssions and permission levels.
        /// </summary>
        /// <param name="permissions_"></param>
        /// <returns></returns>
        internal IEnumerable<Claim> GetFinalPermissions(IEnumerable<Permission> permissions_)
        {
            List<Claim> claims = new List<Claim>();
            // TODO

            return claims;
        }

        /// <summary>
        /// Loads the permissions from database
        /// </summary>
        /// <param name="user_"></param>
        /// <returns></returns>
        public IEnumerable<Permission> LoadPermissions(User user_)
        {
            List<Permission> permissions = new List<Permission>();
            // TODO
            return permissions;
        }
    }
}
