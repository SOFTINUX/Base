// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Security.Common.Attributes;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.Security.ViewModels.Permissions;

using Permission = SoftinuxBase.Security.Common.Enums.Permission;

namespace SoftinuxBase.Security.Controllers
{
    [PermissionRequirement(Permission.Admin)]
    public class GrantPermissionsController : Infrastructure.ControllerBase
    {
        private readonly RoleManager<IdentityRole<string>> _roleManager;

        public GrantPermissionsController(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_) : base(storage_)
        {
            _roleManager = roleManager_;
        }

        #region READ

        [PermissionRequirement(Permission.Admin)]
        [Route("administration/grantpermissions")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Create a dictionary with all roles for injecting as json into grant permission page
            Dictionary<string, IdentityRole<string>> rolesList = new Dictionary<string, IdentityRole<string>>();

            // Create a dictionary with role id and name, since we will use role name in GrantViewModel
            // and we have role id in RolePermission table.
            Dictionary<string, string> roleNameByRoleId = new Dictionary<string, string>();

            foreach (var role in _roleManager.Roles)
            {
                roleNameByRoleId.Add(role.Id, role.Name);
                rolesList.Add(role.Id, role);
            }

            ViewBag.RolesList = rolesList;

            var model = ReadGrants.ReadAll(_roleManager, Storage, roleNameByRoleId);
            return await Task.Run(() => View(model));
        }

        /// <summary>
        /// Return role for edition: role information and associated extensions list.
        /// </summary>
        /// <param name="roleId_"></param>
        /// <returns>Http code and JSON role object.</returns>
        [PermissionRequirement(Permission.Admin)]
        [Route("administration/read-role")]
        [HttpGet]
        public async Task<IActionResult> GetRoleForEdition(string roleId_)
        {
            if (string.IsNullOrWhiteSpace(roleId_) || string.IsNullOrEmpty(roleId_))
            {
                return StatusCode(400, Json("No role id given"));
            }

            var role = await _roleManager.FindByIdAsync(roleId_);

            if (role == null)
            {
                return StatusCode(400, Json("No such role for edition"));
            }

            ReadGrants.GetExtensions(roleId_, Storage, out var availableExtensions, out var selectedExtensions);

            ReadRoleViewModel result = new ReadRoleViewModel
            {
                Role = role,
                SelectedExtensions = selectedExtensions,
                AvailableExtensions = availableExtensions
            };
            return StatusCode(200, Json(result));
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Create a role. Then create a set of records indicating to which extensions with which permission this role is linked to.
        /// </summary>
        /// <param name="model_"></param>
        /// <returns>Http status code.</returns>
        [Route("administration/save-new-role")]
        [HttpPost]
        public async Task<IActionResult> SaveNewRoleAndItsPermissions(SaveNewRoleAndGrantsViewModel model_)
        {
            string error = await CreateRoleAndGrants.CheckAndSaveNewRoleAndGrants(Storage, _roleManager, model_);
            return StatusCode(string.IsNullOrEmpty(error) ? 201 : 400, error);
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Update a record indicating with which permission this role is linked to an extension.
        /// </summary>
        /// <param name="roleName_">Role name.</param>
        /// <param name="permissionValue_">New permission level to save.</param>
        /// <param name="extension_">Extension.</param>
        /// <returns>JSON with "true" when it succeeded.</returns>
        [PermissionRequirement(Permission.Admin)]
        [Route("administration/update-role-permission")]
        [HttpGet] // TODO change to POST
        public async Task<IActionResult> UpdateRolePermission(string roleName_, string permissionValue_, string extension_)
        {
            string roleId = (await _roleManager.FindByNameAsync(roleName_)).Id;
            IRolePermissionRepository repo = Storage.GetRepository<IRolePermissionRepository>();
            repo.Delete(roleId, extension_);

            if (Enum.TryParse<Permission>(permissionValue_, true, out var permissionEnumValue))
            {
                var permissionEntity = Storage.GetRepository<IPermissionRepository>().Find(permissionEnumValue);
                repo.Create(new RolePermission { RoleId = roleId, PermissionId = permissionEntity.Id, Extension = extension_ });
            }

            Storage.Save();
            return new JsonResult(true);
        }

        /// <summary>
        /// Update role name and linked extensions with permission level.
        /// </summary>
        /// <param name="model_"></param>
        /// <returns>Status code 201, or 400 with an error message</returns>
        [PermissionRequirement(Permission.Admin)]
        [Route("administration/update-role")]
        [HttpPost]
        public async Task<IActionResult> UpdateRoleAndItsPermissions(UpdateRoleAndGrantsViewModel model_)
        {
            string error = await UpdateRoleAndGrants.CheckAndUpdateRoleAndGrants(Storage, _roleManager, model_);
            return StatusCode(string.IsNullOrEmpty(error) ? 201 : 400, error);
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Delete the record linking a role to an extension.
        /// </summary>
        /// <param name="roleName_">string represent role to delete.</param>
        /// <param name="extensionName_">string represent the extension name linked to role.</param>
        /// <returns>Status code 204 (ok) or 400 (no deletion occurred).</returns>
        [HttpPost]
        [Route("administration/delete-role-extension")]
        public async Task<IActionResult> DeleteRoleExtensionLink(string roleName_, string extensionName_)
        {
            bool deleted = await Tools.DeleteRole.DeleteRoleExtensionLink(this.Storage, _roleManager, extensionName_, roleName_);
            return StatusCode(deleted ? 204 : 400);
        }

        /// <summary>
        /// Delete the records linking a role to any extension, then delete role record if possible..
        /// </summary>
        /// <param name="roleName_">string represent role name to delete.</param>
        /// <returns>Status code 204, or 400 with an error message.</returns>
        [HttpPost]
        [Route("administration/delete-role")]
        public async Task<IActionResult> DeleteRole(string roleName_)
        {
            string error = await Tools.DeleteRole.DeleteRoleAndAllLinks(this.Storage, _roleManager, roleName_);
            return StatusCode(string.IsNullOrEmpty(error) ? 204 : 400, error);
        }

        #endregion

    }
}