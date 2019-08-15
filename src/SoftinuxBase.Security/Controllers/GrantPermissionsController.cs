// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Common.Attributes;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.Security.ViewModels.Permissions;

using Permission = SoftinuxBase.Security.Common.Enums.Permission;

namespace SoftinuxBase.Security.Controllers
{
    [PermissionRequirement(Permission.Admin, Constants.SoftinuxBaseSecurity)]
    public class GrantPermissionsController : Infrastructure.ControllerBase
    {
        private readonly RoleManager<IdentityRole<string>> _roleManager;

        public GrantPermissionsController(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_) : base(storage_)
        {
            _roleManager = roleManager_;
        }

        #region READ

        [Route("administration/grant-permissions")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        /// <summary>
        /// Return role for edition: role information and associated extensions list.
        /// </summary>
        /// <param name="roleId_">Id of role to read.</param>
        /// <returns>Http code and JSON role object.</returns>
        [Route("administration/read-role")]
        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReadRole(string roleId_)
        {
            if (string.IsNullOrWhiteSpace(roleId_) || string.IsNullOrEmpty(roleId_))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Json("No role id given"));
            }

            var role = await _roleManager.FindByIdAsync(roleId_);

            if (role == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Json("No such role for edition"));
            }

            ReadGrants.GetExtensions(roleId_, Storage, out var availableExtensions, out var selectedExtensions);

            ReadRoleViewModel result = new ReadRoleViewModel
            {
                Role = role,
                SelectedExtensions = selectedExtensions,
                AvailableExtensions = availableExtensions
            };
            return StatusCode((int)HttpStatusCode.OK, Json(result));
        }

        /// <summary>
        /// Return table of permissions for list roles extensions.
        /// </summary>
        /// <returns>view component.</returns>
        [Route("administration/read-permissions-grants")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult ReadPermissionsTable()
        {
            return ViewComponent("GrantPermissions");
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Create a role. Then create a set of records indicating to which extensions with which permission this role is linked to.
        /// </summary>
        /// <param name="model_">object representing values passed from ajax.</param>
        /// <returns>Http status code.</returns>
        [Route("administration/save-new-role")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SaveNewRoleAndItsPermissions([FromBody] SaveNewRoleAndGrantsViewModel model_)
        {
            string error = await CreateRoleAndGrants.CheckAndSaveNewRoleAndGrants(Storage, _roleManager, model_);
            return StatusCode(string.IsNullOrEmpty(error) ? (int)HttpStatusCode.Created : (int)HttpStatusCode.BadRequest, error);
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Update a record indicating with which permission this role is linked to an extension.
        /// </summary>
        /// <param name="model_">object representing values passed from ajax.</param>
        /// <returns>OK 200.</returns>
        [Route("administration/update-role-permission")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRolePermission([FromBody] UpdateRolePermissionViewModel model_)
        {
            string roleId = (await _roleManager.FindByNameAsync(model_.RoleName)).Id;
            IRolePermissionRepository repo = Storage.GetRepository<IRolePermissionRepository>();
            repo.Delete(roleId, model_.Extension);

            if (Enum.TryParse<Permission>(model_.PermissionValue, true, out var permissionEnumValue))
            {
                var permissionEntity = Storage.GetRepository<IPermissionRepository>().Find(permissionEnumValue);
                repo.Create(new RolePermission { RoleId = roleId, PermissionId = permissionEntity.Id, Extension = model_.Extension });
            }

            Storage.SaveAsync();
            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Update role name and linked extensions with permission level.
        /// </summary>
        /// <param name="model_">object representing values passed from ajax.</param>
        /// <returns>Status code 201, or 400 with an error message.</returns>
        [Route("administration/update-role")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRoleAndItsPermissions([FromBody] UpdateRoleAndGrantsViewModel model_)
        {
            string error = await UpdateRoleAndGrants.CheckAndUpdateRoleAndGrants(Storage, _roleManager, model_);
            return StatusCode(string.IsNullOrEmpty(error) ? (int)HttpStatusCode.Created : (int)HttpStatusCode.BadRequest, error);
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Delete the record linking a role to an extension.
        /// </summary>
        /// <param name="model_">object representing values passed from ajax.</param>
        /// <returns>Status code 204 (ok) or 400 (no deletion occurred).</returns>
        [HttpDelete]
        [Route("administration/delete-role-extension")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRoleExtensionLink([FromBody] DeleteRoleExtensionLinkViewModel model_)
        {
            //bool deleted = await Tools.DeleteRole.DeleteRoleExtensionLink(this.Storage, _roleManager, model_.ExtensionName, model_.RoleName);
            //return StatusCode(deleted ? (int)HttpStatusCode.NoContent : (int)HttpStatusCode.BadRequest);
            return StatusCode(403);
        }

        /// <summary>
        /// Delete the records linking a role to any extension, then delete role record if possible..
        /// </summary>
        /// <param name="roleName_">Name of role to delete.</param>
        /// <returns>Status code 204, or 400 with an error message.</returns>
        [HttpDelete]
        [Route("administration/delete-role")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRole(string roleName_)
        {
            string error = await Tools.DeleteRole.DeleteRoleAndAllLinks(this.Storage, _roleManager, roleName_);
            return StatusCode(string.IsNullOrEmpty(error) ? (int)HttpStatusCode.NoContent : (int)HttpStatusCode.BadRequest, error);
        }

        #endregion

    }
}