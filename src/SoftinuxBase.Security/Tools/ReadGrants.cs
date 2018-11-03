// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Infrastructure.Extensions;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.ViewModels.Permissions;

namespace SoftinuxBase.Security.Tools
{
    public static class ReadGrants
    {
        /// <summary>
        /// Read all grants: to have a global view of permissions granting: for a role or a user, what kind of permission is granted, for every scope (extension).
        /// </summary>
        /// <returns></returns>
        public static GrantViewModel ReadAll(RoleManager<IdentityRole<string>> roleManager_, IStorage storage_, Dictionary<string, string> roleNameByRoleId_)
        {
            GrantViewModel model = new GrantViewModel();

              
             // 1. Get all scopes from available extensions, create initial dictionaries
            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
                model.PermissionsByRoleAndScope.Add(extensionMetadata.GetScope(), new Dictionary<string, List<global::SoftinuxBase.Security.Common.Enums.Permission>>());
            }

            // 2. Read data from RolePermission table
            // Names of roles that have permissions attributed
            HashSet<string> rolesWithPerms = new HashSet<string>();

            // Read role/permission/extension settings
            List<RolePermission> allRp = storage_.GetRepository<IRolePermissionRepository>().All();
            foreach (RolePermission rp in allRp)
            {
                if (!model.PermissionsByRoleAndScope.ContainsKey(rp.Scope))
                {
                    // A database record related to a not loaded extension (scope). Ignore this.
                    continue;
                }
                string roleName = roleNameByRoleId_.ContainsKey(rp.RoleId) ? roleNameByRoleId_[rp.RoleId] : null;
                if (!model.PermissionsByRoleAndScope[rp.Scope].ContainsKey(roleName))
                    model.PermissionsByRoleAndScope[rp.Scope].Add(roleName, new List<global::SoftinuxBase.Security.Common.Enums.Permission>());
                // Format the list of Permission enum values according to DB enum value
                model.PermissionsByRoleAndScope[rp.Scope][roleName] = PermissionHelper.GetLowerOrEqual(PermissionHelper.FromId(rp.PermissionId));
                rolesWithPerms.Add(roleName);
            }

            // 3. Also read roles for which no permissions were set
            IList<string> roleNames = roleManager_.Roles.Select(r_ => r_.Name).ToList();
            foreach (string role in roleNames)
            {
                if (rolesWithPerms.Contains(role))
                    continue;
                foreach (string scope in model.PermissionsByRoleAndScope.Keys)
                {
                    model.PermissionsByRoleAndScope[scope].Add(role, new List<global::SoftinuxBase.Security.Common.Enums.Permission>());
                }
            }

            return model;
        }

        /// <summary>
        /// Get the name of all the extensions linked to a role.
        /// </summary>
        /// <param name="roleId_">Id of a role</param>
        /// <param name="storage_"></param>
        /// <returns></returns>
        public static List<string> GetExtensions(string roleId_, IStorage storage_)
        {
            return storage_.GetRepository<IRolePermissionRepository>().FilteredByRoleId(roleId_).Select(rp_ => rp_.Scope)
                .ToList();

        }
    }
}