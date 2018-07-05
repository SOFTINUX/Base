using System;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.ViewModels.Permissions;

namespace Security.Tools
{
    public static class RoleTools
    {
        /// <summary>
        /// Check that a role with a close name exists.
        /// Compare role names ignoring case and spaces, so that
        /// "Role 1" and "role1" are considered homonyms (too close).
        /// </summary>
        /// <param name="roleName_"></param>
        /// <returns>true when a role with a close name is found</returns>
        public static bool CheckThatRoleWithCloseNameExists(string roleName_)
        {
            // TODO
            return false;
        }

        /// <summary>
        /// First, check that a role with a close name doesn't already exist.
        /// Second, save new data into database.
        /// </summary>
        /// <param name="model_"></param>
        /// <returns>Not null when something failed, else null when save went ok</returns>
        public async static Task<string> CheckAndSaveNewRole(SaveNewRoleViewModel model_, RoleManager<IdentityRole<string>> roleManager_, IStorage storage_)
        {
            if (CheckThatRoleWithCloseNameExists(model_.Role))
            {
                return "A role with a close name already exists";
            }

            try
            {
                // Convert the string to the enum
                var permissionEnum = Enum.Parse<Security.Common.Enums.Permission>(model_.Permission, true);

                // Save the Role
                IdentityRole<string> identityRole = new IdentityRole<string>
                {
                    Id = model_.Role,
                    Name = model_.Role
                };
                await roleManager_.CreateAsync(identityRole);

                // Save the role-extension-permission link
                IRolePermissionRepository repo = storage_.GetRepository<IRolePermissionRepository>();
                foreach (string extension in model_.Extensions)
                {
                    repo.Create(new RolePermission { RoleId = model_.Role, PermissionId = permissionEnum.ToString(), Scope = extension });
                }
                return null;

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}