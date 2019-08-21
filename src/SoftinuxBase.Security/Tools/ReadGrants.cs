// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.ViewModels.Permissions;
using Permission = SoftinuxBase.Security.Common.Enums.Permission;

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
                model.PermissionsByRoleAndExtension.Add(extensionMetadata.Name, new Dictionary<string, List<global::SoftinuxBase.Security.Common.Enums.Permission>>());
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
                var extensionName = extensionMetadata.Name;
                if (!selectedExtensionsNames.Contains(extensionName) && extensionMetadata.IsAvailableForPermissions)
                {
                    availableExtensions_.Add(extensionName);
                }
            }
        }

        /// <summary>
        /// This function checks that the role is the last grant of Admin permission level to the target extension.
        /// This allows to warn the user in case no user is granted Admin for this extension and we want to remove the grant from role.
        /// In case the extension is SoftinuxBase.Security, this check will be used to prevent the delete action.
        /// </summary>
        /// <param name="roleManager_">ASP.NET Core identity role manager.</param>
        /// <param name="storage_">The data storage instance.</param>
        /// <param name="roleName_">Role name.</param>
        /// <param name="extensionName_">Name of extension.</param>
        /// <returns>bool.</returns>
        public static async Task<bool> IsRoleLastAdminPermissionLevelGrantForExtensionAsync(RoleManager<IdentityRole<string>> roleManager_, IStorage storage_, string roleName_, string extensionName_)
        {
            // test
            var currentRole = await roleManager_.FindByNameAsync(roleName_);

            // end test

            // Is there a user directly granted Admin for this extension?
            if (storage_.GetRepository<IUserPermissionRepository>().FindBy(extensionName_, Permission.Admin) != null)
            {
                // A user is directly granted
                return false;
            }

            var rolePermissionRecordsWithAdminLevel = storage_.GetRepository<IRolePermissionRepository>().FindBy(extensionName_, Permission.Admin);

            if (!rolePermissionRecordsWithAdminLevel.Any())
            {
                // This method shouldn't have been called in this case :-)
                return false;
            }

            // var currentRole = await roleManager_.FindByNameAsync(roleName_);
            if (rolePermissionRecordsWithAdminLevel.Count() == 1 && rolePermissionRecordsWithAdminLevel.First().Id == currentRole.Id)
            {
                return true;
            }

            IEnumerable<User> usersHavingCurrentRole =
                storage_.GetRepository<IAspNetUsersRepository>().FindActiveUsersHavingRole(roleName_);

            // TODO change the FindActiveUsersHavingRole to pass a list of role names.

            /*
             The roles that have Admin right must have users linked to them
             and if at least one user found => return false, else true
            */

            return false;
        }
    }
}