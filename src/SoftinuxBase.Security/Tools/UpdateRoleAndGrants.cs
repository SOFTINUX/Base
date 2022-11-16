// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Permissions;

[assembly: InternalsVisibleTo("SoftinuxBase.SecurityTests")]

namespace SoftinuxBase.Security.Tools
{
    /// <summary>
    /// The UpdateRoleAndGrants class contains all methods to update roles and grants.
    /// </summary>
    /// <remarks>
    /// This class is internal to SoftinuxBase Security.
    /// </remarks>
    internal static class UpdateRoleAndGrants
    {
        /// <summary>
        /// Update or create the RoleToPermissions record, for the given role, adding or removing a permission.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="rolesManager_">Base's custom interface to <see cref="RoleManager{TRole}"/>.</param>
        /// <param name="roleName_">Role Name.</param>
        /// <param name="extensionName_">Name of the extension, which a Type is associated to hold the permissions.</param>
        /// <param name="permissionValue_">Permission value (in extension's permissions enum type).</param>
        /// <param name="add_">True to add the permission, false to remove.</param>
        /// <returns>Error message, else null.</returns>
        internal static async Task<string> UpdateRoleToPermissionsAsync(IStorage storage_, IAspNetRolesManager rolesManager_, string roleName_, string extensionName_, short permissionValue_, bool add_)
        {
            // Lookup for the extension permissions Type.
            var extensionMetadata = ExtensionManager.GetInstances<IExtensionMetadata>().FirstOrDefault((em_) => em_.Name == extensionName_);
            if (extensionMetadata == null)
            {
                return $"Extension {extensionName_} does not exist";
            }

            if (extensionMetadata.Permissions == null)
            {
                return $"Extension {extensionName_} doesn't define a Type to hold permissions";
            }

            // Lookup for the role and RoleToPermission
            var role = await rolesManager_.FindByNameAsync(roleName_);
            if (role == null)
            {
                return $"Role {roleName_} does not exist";
            }

            var repository = storage_.GetRepository<IRoleToPermissionsRepository>();
            var roleToPermissions = repository.FindBy(roleName_);
            if (roleToPermissions == null)
            {
                if (!add_)
                {
                    return $"Cannot remove permission for role {roleName_} that has no permissions";
                }

                roleToPermissions = new RoleToPermissions(roleName_, roleName_, new PermissionsDictionary());
                repository.Create(roleToPermissions);
            }

            // Unpack and update the permissions dictionary
            var permissionDictionary = roleToPermissions.PermissionsForRole;
            if (add_)
            {
                var result = permissionDictionary.Add(extensionMetadata.Permissions, permissionValue_);
                if (!result)
                {
                    return "Permission was already associated to role";
                }
            }
            else
            {
                var result = permissionDictionary.Remove(extensionMetadata.Permissions, permissionValue_);
                if (!result)
                {
                    return "Permission was already not associated to role anymore";
                }
            }

            // And finally save back to database...
            repository.SetPermissions(roleName_, permissionDictionary);
            await storage_.SaveAsync();

            return null;
        }

        /// <summary>
        /// Update the RoleToPermissions record, when it exists, for the given role, when role name changes.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="roleName_">Role Name.</param>
        /// <param name="newRoleName_">New role name.</param>
        /// <returns>Success or not found indicator.</returns>
        internal static async Task<bool?> UpdateRoleToPermissionsAsync(IStorage storage_, string roleName_, string newRoleName_)
        {
            var repository = storage_.GetRepository<IRoleToPermissionsRepository>();
            var roleToPermissions = repository.FindBy(roleName_);
            if (roleToPermissions == null)
            {
                // Nothing to update
                return null;
            }

            var newRoleToPermissions = roleToPermissions.Copy(newRoleName_);
            repository.Delete(roleToPermissions);
            repository.Create(newRoleToPermissions);
            
            // And finally save back to database...
            await storage_.SaveAsync();

            return true;
        }
    }
}