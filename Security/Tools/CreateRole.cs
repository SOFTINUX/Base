// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.ViewModels.Permissions;

namespace Security.Tools
{
    public static class CreateRole
    {
        /// <summary>
        /// First, check that a role with a close name doesn't already exist.
        /// Second, save new data into database.
        /// </summary>
        /// <param name="model_"></param>
        /// <returns>Not null when something failed, else null when save went ok</returns>
        public async static Task<string> CheckAndSaveNewRole(SaveNewRoleViewModel model_, RoleManager<IdentityRole<string>> roleManager_, IStorage storage_)
        {
            if (UpdateRole.CheckThatRoleOfThisNameExists(model_.Role))
            {
                return "A role with this name already exists";
            }

            try
            {
                // Convert the string to the enum
                var permissionEnum = Enum.Parse<Security.Common.Enums.Permission>(model_.Permission, true);

                // Save the Role
                IdentityRole<string> identityRole = new IdentityRole<string>
                {
                    // Auto-incremented ID
                    Name = model_.Role
                };
                await roleManager_.CreateAsync(identityRole);

                // Save the role-extension-permission link
                IRolePermissionRepository repo = storage_.GetRepository<IRolePermissionRepository>();
                foreach (string extension in model_.Extensions)
                {
                    repo.Create(new RolePermission { RoleId = model_.Role, PermissionId = permissionEnum.ToString(), Scope = extension });
                }
                storage_.Save();
                
                return null;

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}