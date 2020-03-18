// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Security.ViewModels.Permissions;
using Permission = SoftinuxBase.Security.Permissions.Enums.Permission;

namespace SoftinuxBase.Security.Tools
{
    /*
        The main ReadGrants class.
        Contains all methods for reading grants permissions.
    */
    /// <summary>
    /// The main ReadGrants class.
    ///
    /// contains all methods for reading grants permissions.
    /// </summary>
    public static class ReadGrants
    {
        /// <summary>
        /// Read all grants:
        /// 
        /// - to have a global view of permissions granting:
        /// 
        /// -- for a role or a user, what kind of permission is granted, for every extension.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <returns>Return a GrantViewModel model object.</returns>
        public static GrantViewModel ReadAll(IStorage storage_)
        {
            GrantViewModel model = new GrantViewModel();
            // key : extension's permission enum type assembly-qualified name, since the enum can be in another assembly.
            // value : extension name.
            var enumAndExtensionDic = new Dictionary<string, string>();

            // 1. Get all extension names from loaded extensions, create initial dictionaries
            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
                model.PermissionsByRoleAndExtension.Add(extensionMetadata.Name, new Dictionary<string, List<PermissionDisplay>>());
                enumAndExtensionDic.Add(extensionMetadata.Permissions.AssemblyQualifiedName, extensionMetadata.Name);
            }

            // 2. Read role/permission/extension settings (RoleToPermissions table)
            List<RoleToPermissions> rolesToPermissions = storage_.GetRepository<IRoleToPermissionsRepository>().All().ToList();
            foreach (RoleToPermissions roleToPermission in rolesToPermissions)
            {
                var permissions = roleToPermission.PermissionsForRole;
                PermissionsDisplayDictionary permissionsDisplayDictionary = new PermissionsDisplayDictionary(permissions);
                foreach (var permissionEnumTypeAssemblyQualifiedName in permissions.Dictionary.Keys)
                {
                    enumAndExtensionDic.TryGetValue(permissionEnumTypeAssemblyQualifiedName, out var extensionName);
                    if (!model.PermissionsByRoleAndExtension.ContainsKey(extensionName))
                    {
                        // A database record related to a not loaded extension. Ignore this.
                        continue;
                    }

                    model.PermissionsByRoleAndExtension[extensionName].Add(roleToPermission.RoleName, permissionsDisplayDictionary.Get(permissionEnumTypeAssemblyQualifiedName).ToList());
                }
            }

            return model;
        }

        // TODO REWRITE
        /// <summary>
        /// Get the list of all extensions associated to a role, with corresponding permissions,
        /// and also the list of extensions not linked to the role.
        /// </summary>
        /// <param name="roleId_">Id of a role.</param>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="availableExtensions_">Output a list of available extensions.</param>
        /// <param name="selectedExtensions_">Output a list of selected extensions.</param>
        public static void GetExtensions(string roleId_, IStorage storage_, out IList<string> availableExtensions_, out IList<SelectedExtension> selectedExtensions_)
        {
            selectedExtensions_ = storage_.GetRepository<IRolePermissionRepository>().FilteredByRoleId(roleId_).Select(
                    rp_ => new SelectedExtension { ExtensionName = rp_.Extension, PermissionName = rp_.Permission.Name, PermissionId = rp_.PermissionId })
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

        // TODO REWRITE
        /// <summary>
        /// This function checks that the role is the last grant of Admin permission level to the target extension.
        ///
        /// This allows to warn the user in case no user would be granted Admin anymore for this extension after we remove the grant from this role.
        ///
        /// In case the extension is SoftinuxBase.Security, this check will be used to prevent the delete action.
        ///
        /// Rules:
        /// <ul>
        /// <li>No user should be granted admin permission level for this role and extension.</li>
        /// <li>The other roles linked to the extension, with Admin permission level, should be an empty list or not linked to aany user.</li>
        /// </ul>
        /// </summary>
        /// <param name="roleManager_">ASP.NET Core identity role manager.</param>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="roleName_">Role name.</param>
        /// <param name="extensionName_">Name of extension.</param>
        /// <returns>True when the role is the last grant of Admin permission level to the target extension.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629", Justification = "Suppress warning for 'Rules:' in summary")]
        public static async Task<bool> IsRoleLastAdminPermissionLevelGrantForExtensionAsync(RoleManager<IdentityRole<string>> roleManager_, IStorage storage_, string roleName_, string extensionName_)
        {
            var currentRole = await roleManager_.FindByNameAsync(roleName_);

            if (currentRole == null)
            {
                // The role has been deleted by someone else
                return false;
            }

            // Is there a user directly granted Admin for this extension?
            if (storage_.GetRepository<IUserPermissionRepository>().FindBy(extensionName_, Permission.Admin).Any())
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

            if (rolePermissionRecordsWithAdminLevel.Count() == 1 && rolePermissionRecordsWithAdminLevel.First().Id == currentRole.Id)
            {
                // There is only one grant to current role
                return true;
            }

            /*
            The roles that have Admin right must have users linked to them
            and if at least one user found => return false, else true
            */

            IEnumerable<User> usersHavingRoles =
                storage_.GetRepository<IAspNetUsersRepository>().FindUsersHavingRoles(rolePermissionRecordsWithAdminLevel.Where(rp_ => rp_.RoleId != currentRole.Id).Select(rp_ => rp_.Role.NormalizedName));

            return !usersHavingRoles.Any();
        }
    }
}