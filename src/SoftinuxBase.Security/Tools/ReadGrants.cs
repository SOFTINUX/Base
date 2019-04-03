// Copyright © 2017-2019 SOFTINUX. All rights reserved.
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
    /*
        The main ReadGrants class
        Contains all methods for reading grants permissions
    */
    public static class ReadGrants
    {
        /// <summary>
        /// Read all grants: to have a global view of permissions granting: for a role or a user, what kind of permission is granted, for every scope (extension).
        /// </summary>
        /// <param name="roleManager_">role manager instance.</param>
        /// <param name="storage_">the data storage instance.</param>
        /// <param name="roleNameByRoleId_">dictionary of all role with id.</param>
        /// <returns>return a GrantViewModel model object.</returns>
        public static GrantViewModel ReadAll(RoleManager<IdentityRole<string>> roleManager_, IStorage storage_, Dictionary<string, string> roleNameByRoleId_)
        {
            GrantViewModel model = new GrantViewModel();

             // 1. Get all scopes from available extensions, create initial dictionaries
            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
                model.PermissionsByRoleAndExtension.Add(extensionMetadata.GetExtensionName(), new Dictionary<string, List<global::SoftinuxBase.Security.Common.Enums.Permission>>());
            }

            // 2. Read data from RolePermission table
            // Names of roles that have permissions attributed
            HashSet<string> rolesWithPerms = new HashSet<string>();

            // Read role/permission/extension settings
            List<RolePermission> allRp = storage_.GetRepository<IRolePermissionRepository>().All().ToList();
            foreach (RolePermission rp in allRp)
            {
                if (!model.PermissionsByRoleAndExtension.ContainsKey(rp.Extension))
                {
                    // A database record related to a not loaded extension (scope). Ignore this.
                    continue;
                }

                string roleName = roleNameByRoleId_.ContainsKey(rp.RoleId) ? roleNameByRoleId_[rp.RoleId] : null;
                if (!model.PermissionsByRoleAndExtension[rp.Extension].ContainsKey(roleName))
                {
                    model.PermissionsByRoleAndExtension[rp.Extension].Add(roleName, new List<global::SoftinuxBase.Security.Common.Enums.Permission>());
                }

                // Format the list of Permission enum values according to DB enum value
                model.PermissionsByRoleAndExtension[rp.Extension][roleName] = PermissionHelper.GetLowerOrEqual(PermissionHelper.FromName(rp.Permission.Name));
                rolesWithPerms.Add(roleName);
            }

            // 3. Also read roles for which no permissions were set
            IList<string> roleNames = roleManager_.Roles.Select(r_ => r_.Name).ToList();
            foreach (string role in roleNames)
            {
                if (rolesWithPerms.Contains(role))
                {
                    continue;
                }

                foreach (string scope in model.PermissionsByRoleAndExtension.Keys)
                {
                    model.PermissionsByRoleAndExtension[scope].Add(role, new List<global::SoftinuxBase.Security.Common.Enums.Permission>());
                }
            }

            return model;
        }

        /// <summary>
        /// Get the list of extensions associated to a role, with corresponding permission, and also the list of extensions not linked to the role.
        /// </summary>
        /// <param name="roleId_">Id of a role.</param>
        /// <param name="storage_">the data storage instance.</param>
        /// <param name="availableExtensions_">output a list of available extensions.</param>
        /// <param name="selectedExtensions_">output a list of selected extensions.</param>
        public static void GetExtensions(string roleId_, IStorage storage_, out IList<string> availableExtensions_, out IList<SelectedExtension> selectedExtensions_)
        {
            selectedExtensions_ = storage_.GetRepository<IRolePermissionRepository>().FilteredByRoleId(roleId_).Select(
                rp_ => new SelectedExtension
                {
                    ExtensionName = rp_.Extension, 
                    PermissionName = rp_.Permission.Name, 
                    PermissionId = rp_.PermissionId
                })
                .ToList();

            IEnumerable<string> selectedExtensionsNames = selectedExtensions_.Select(se_ => se_.ExtensionName).ToList();

            availableExtensions_ = new List<string>();
            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
                var extensionName = extensionMetadata.GetExtensionName();
                if (!selectedExtensionsNames.Contains(extensionName) && extensionMetadata.IsAvailableForPermissions)
                {
                    availableExtensions_.Add(extensionName);
                }
            }
        }

        public static bool HasAnyUserDirectAdminPermission()
        {
            // TODO
            // check user-permission table
            return false;
        }

        public static bool HasOtherRoleAdminPermission(string roleId_)
        {
            // TODO
            // check role-permission tables
            // Remove record associated to param role
            return false;
        }
    }
}