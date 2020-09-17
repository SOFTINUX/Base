// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Security.ViewModels.Permissions;

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
        /// - to have a global view of grantable permissions and their effective granting:
        ///
        /// -- for a role or a user, what kind of permission is granted, for every extension.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <returns>Return a GrantViewModel model object.</returns>
        public static GrantViewModel ReadAll(IStorage storage_)
        {
            GrantViewModel model = new GrantViewModel();

            // key : extension's permission enum type assembly-qualified name, since the enum can be in another assembly,
            // value : extension name
            var extensionEnumAndNameDict = new Dictionary<string, string>();

            // 1. Get all extension names from loaded extensions, create initial dictionaries
            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
                if (extensionMetadata.Permissions == null)
                {
                    // Ignore extension when it doesn't define its permissions
                    continue;
                }

                // 1a. Create the dictionary entry for the extension
                model.RolesWithPermissions.Add(extensionMetadata.Name, new Dictionary<PermissionDisplay, List<string>>());
                extensionEnumAndNameDict.Add(extensionMetadata.Permissions.AssemblyQualifiedName, extensionMetadata.Name);

                // 1b. Build the permissions displays
                var permissionDisplays = PermissionDisplay.GetPermissionsToDisplay(extensionMetadata.Name, extensionMetadata.Permissions).ToHashSet();

                // 1c. Create the list of roles entry, for the permission display
                foreach (var permissionDisplay in permissionDisplays)
                {
                    model.RolesWithPermissions[permissionDisplay.ExtensionName].Add(permissionDisplay, new List<string>());
                }
            }

            // 2. Read role/permission/extension settings (RoleToPermissions table) to add the role items
            List<RoleToPermissions> rolesToPermissions = storage_.GetRepository<IRoleToPermissionsRepository>().All().ToList();

            foreach (RoleToPermissions roleToPermission in rolesToPermissions)
            {
                model.RoleNames.Add(roleToPermission.RoleName);

                var permissions = roleToPermission.PermissionsForRole;

                // Loop over all permission enums given by loaded extensions
                foreach (var permissionEnumType in extensionEnumAndNameDict.Keys)
                {
                    extensionEnumAndNameDict.TryGetValue(permissionEnumType, out var extensionName);
                    if (extensionName == null)
                    {
                        // Record related to a removed extension
                        // TODO add unit test case
                        continue;
                    }

                    permissions.Dictionary.TryGetValue(permissionEnumType, out var permissionValues);
                    if (permissionValues == null)
                    {
                        // TODO add unit test case
                        continue;
                    }

                    foreach (var permissionValue in permissionValues)
                    {
                        var permissionDisplay = model.RolesWithPermissions[extensionName].Keys.FirstOrDefault(permissionDisplay_ => permissionDisplay_.PermissionEnumValue == permissionValue);
                        if (permissionDisplay == null)
                        {
                            // A granted permission value that does not correspond to a permission display: not managed value
                            continue;
                        }

                        model.RolesWithPermissions[extensionName].TryGetValue(permissionDisplay, out var roleNames);
                        if (roleNames == null)
                        {
                            roleNames = new List<string>();
                            model.RolesWithPermissions[extensionName].Add(permissionDisplay, roleNames);
                        }

                        roleNames.Add(roleToPermission.RoleName);
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Gets the list of all extensions associated to a role, with corresponding permissions,
        /// and also the list of extensions not linked to the role.
        /// </summary>
        /// <param name="roleName_">Name of a role.</param>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="availableExtensions_">Output a list of available extensions.</param>
        /// <param name="selectedExtensions_">Output a list of selected extensions.</param>
        public static void GetExtensions(string roleName_, IStorage storage_, out IList<string> availableExtensions_, out IList<SelectedExtension> selectedExtensions_)
        {
            var rolePermissions = storage_.GetRepository<IRoleToPermissionsRepository>().FindBy(roleName_)?.PermissionsForRole;
            availableExtensions_ = new List<string>();
            selectedExtensions_ = new List<SelectedExtension>();

            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
                if (extensionMetadata.Permissions == null)
                {
                    continue;
                }

                if (rolePermissions != null && rolePermissions.Contains(extensionMetadata.Permissions.AssemblyQualifiedName))
                {
                    // The extension permission enum is present
                    var selectedExtension = new SelectedExtension(extensionMetadata.Name);
                    selectedExtensions_.Add(selectedExtension);
                    var permissionDisplays = PermissionDisplay.GetPermissionsToDisplay(extensionMetadata.Name, extensionMetadata.Permissions).ToHashSet();

                    foreach (var permissionDisplay in permissionDisplays)
                    {
                        // Permission section initialization if necessary
                        selectedExtension.GroupedBySectionPermissionDisplays.TryGetValue(permissionDisplay.Section, out var selectablePermissionDisplays);
                        if (selectablePermissionDisplays == null)
                        {
                            selectablePermissionDisplays = new List<SelectablePermissionDisplay>();
                            selectedExtension.GroupedBySectionPermissionDisplays.Add(permissionDisplay.Section, selectablePermissionDisplays);
                        }

                        // SelectablePermissionDisplay
                        var selectablePermissionDisplay = new SelectablePermissionDisplay(permissionDisplay, rolePermissions.Contains(extensionMetadata.Permissions.AssemblyQualifiedName, permissionDisplay.PermissionEnumValue));
                        selectablePermissionDisplays.Add(selectablePermissionDisplay);
                    }
                }
                else
                {
                    // The extension permission enum is absent
                    availableExtensions_.Add(extensionMetadata.Name);
                }
            }
        }
    }
}