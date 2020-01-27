// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX.
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Data.EntityFramework;
using SoftinuxBase.Security.Permissions;

namespace SoftinuxBase.Security.FeatureAuthorize
{
    /// <summary>
    /// This is the code that calculates what feature permissions a user has
    /// </summary>
    public class CalcAllowedPermissions
    {
        private readonly ApplicationStorageContext _context;

        public CalcAllowedPermissions(ApplicationStorageContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This is called if the Permissions that a user needs calculating.
        /// It looks at what permissions the user has, and then filters out any permissions
        /// they aren't allowed because they haven't get access to the module that permission is linked to.
        /// The permissions are read from database, unpacked, then repacked because the claim stores a string value.
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns>a string containing the packed permissions</returns>
        public async Task<string> CalcPermissionsForUserAsync(string userId_)
        {
            //This gets all the permissions, with a distinct to remove duplicates
            IEnumerable<PermissionsDictionary> permissionsForUser = (await _context.UserToRoles.Where(x => x.UserId == userId_)
                    .Select(userToRole_ => userToRole_.Role.PermissionsForRole)
                    .ToListAsync());

            PermissionsDictionary allPermissions = PermissionsDictionary.Merge(permissionsForUser.ToArray());

            // TODO migrate the following code, here too we need to manage an enum for every extension
            ////we get the modules this user is allowed to see
            //var userModules = _context.ModulesForUsers.Find(userId)
            //                      ?.AllowedPaidForModules ?? PaidForModules.None;
            ////Now we remove permissions that are linked to modules that the user has no access to
            //var filteredPermissions =
            //    from permission in permissionsForUser
            //    let moduleAttr = typeof(Permissions).GetMember(permission.ToString())[0]
            //        .GetCustomAttribute<LinkedToModuleAttribute>()
            //    where moduleAttr == null || userModules.HasFlag(moduleAttr.PaidForModule)
            //    select permission;

            //return filteredPermissions.PackPermissions();

            return allPermissions.PackPermissions().ToPackedString();
        }

    }
}