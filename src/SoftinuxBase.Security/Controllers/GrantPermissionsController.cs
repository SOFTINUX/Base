// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Barebone.Extensions;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.FeatureAuthorize.PolicyCode;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.Security.ViewModels.Permissions;

namespace SoftinuxBase.Security.Controllers
{
    public class GrantPermissionsController : Infrastructure.ControllerBase
    {
        private readonly IAspNetRolesManager _aspNetRolesManager;

        public GrantPermissionsController(IStorage storage_, IAspNetRolesManager aspNetRolesManager_) : base(storage_)
        {
            _aspNetRolesManager = aspNetRolesManager_;
        }

        #region READ

        [Route("administration/grant-permissions")]
        [HttpGet]
        [ActionName("Index")]
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.ReadRoles)]
        public async Task<IActionResult> IndexAsync()
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
        [ActionName("ReadRole")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.ReadRoles)]
        public async Task<IActionResult> ReadRoleAsync(string roleId_)
        {
            if (string.IsNullOrWhiteSpace(roleId_) || string.IsNullOrEmpty(roleId_))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Json("No role id given"));
            }

            var role = await _aspNetRolesManager.FindByIdAsync(roleId_);

            if (role == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Json("No such role for edition"));
            }

            ReadGrants.GetExtensions(role.Name, Storage, out var availableExtensions, out var selectedExtensions);

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
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.ReadRoles)]
        public IActionResult ReadPermissionsTable()
        {
            return ViewComponent("GrantPermissions");
        }

        /// <summary>
        /// Return updated roles list.
        /// </summary>
        /// <returns>view component.</returns>
        [Route("administration/edit-role-tab")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.ReadRoles)]
        public IActionResult RefreshRoleTab()
        {
            return ViewComponent("EditRolePermissions");
        }

        /// <summary>
        /// Return bulk delete roles list.
        /// </summary>
        /// <returns>view component.</returns>
        [Route("administration/bulk-delete-role-tab")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.ReadRoles)]
        public IActionResult RefreshBulkDeleteTab()
        {
            return ViewComponent("SelectOptionsListRoles");
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Create a role.
        /// </summary>
        /// <param name="model_">object representing values passed from ajax.</param>
        /// <returns>Http status code.</returns>
        [Route("administration/save-new-role")]
        [HttpPost]
        [ActionName("SaveNewRole")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.CreateRoles)]
        public async Task<IActionResult> SaveNewRoleAsync([FromBody] SaveNewRoleViewModel model_)
        {
            string error = await CreateRole.CheckAndSaveNewRoleAsync(_aspNetRolesManager, model_);
            return StatusCode(string.IsNullOrEmpty(error) ? (int)HttpStatusCode.Created : (int)HttpStatusCode.BadRequest, error);
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Add or remove a role from a permission.
        /// </summary>
        /// <param name="model_">object representing values passed from ajax.</param>
        /// <returns>Status code 204 (no content) when update is ok or 400 (update went wrong, with message).</returns>
        [Route("administration/update-role-permission")]
        [HttpPost]
        [ActionName("UpdateRolePermission")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.EditRoles)]
        public async Task<IActionResult> UpdateRoleToPermissionsAsync([FromBody] UpdateRolePermissionViewModel model_)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState.AllErrors());
            }

            var result = await UpdateRoleAndGrants.UpdateRoleToPermissionsAsync(Storage, _aspNetRolesManager, model_.RoleName, model_.ExtensionName, model_.PermissionValue, model_.Add);

            return result == null ? (IActionResult)StatusCode((int)HttpStatusCode.NoContent) : new BadRequestObjectResult(result);
        }

        /// <summary>
        /// Update role name and linked extensions with permission level.
        /// </summary>
        /// <param name="model_">object representing values passed from ajax.</param>
        /// <returns>Status code 201, or 400 with an error message.</returns>
        [Route("administration/update-role")]
        [HttpPost]
        [ActionName("UpdateRoleAndItsPermissions")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.EditRoles)]
        public async Task<IActionResult> UpdateRoleAndItsPermissionsAsync([FromBody] UpdateRoleAndGrantsViewModel model_)
        {
            string error = await UpdateRoleAndGrants.CheckAndUpdateRoleAndGrantsAsync(Storage, _aspNetRolesManager, model_);
            return StatusCode(string.IsNullOrEmpty(error) ? (int)HttpStatusCode.Created : (int)HttpStatusCode.BadRequest, error);
        }

        #endregion

        #region DELETE

        // ReSharper disable InconsistentNaming

        /// <summary>
        /// Delete the record linking a role to an extension.
        /// </summary>
        /// <param name="RoleName">Name of role to delete.</param>
        /// <param name="ExtensionName">Name of linked extension.</param>
        /// <returns>Status code 204 (ok) or 400 (no deletion occurred).</returns>
        [HttpDelete]
        [ActionName("UnlinkRoleExtensionLink")]
        [Route("administration/unlink-role-extension/{RoleName}/{ExtensionName}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1313", Justification = "Ignore camelcase parameters")]
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.EditRoles)]
        public async Task<IActionResult> DeleteRoleExtensionLinkAsync(string RoleName, string ExtensionName)
        {
            bool? deleted = await DeleteRole.DeleteRoleExtensionLinkAsync(this.Storage, ExtensionName, RoleName);
            switch (deleted)
            {
                case true:
                    return StatusCode((int)HttpStatusCode.NoContent);
                case false:
                    return StatusCode((int)HttpStatusCode.BadRequest, "Link not deleted, the role is the last Admin grant to SoftinuxBase.Security extension");
                default:
                    return StatusCode((int)HttpStatusCode.BadRequest, "Role or link not found");
            }
        }

        /// <summary>
        /// Remove link role on all extensions.
        /// </summary>
        /// <param name="RoleName">Role name to unlink on all extension.</param>
        /// <returns>status code.</returns>
        [HttpDelete]
        [ActionName("UnlinkRoleAllExtensions")]
        [Route("administration/unlink-role-all-extensions/{RoleName}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1313", Justification = "Ignore camelcase parameters")]
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.EditRoles)]
        public async Task<IActionResult> UnlinkRoleOnAllExtensions(string RoleName)
        {
            bool? deleted = await DeleteRole.DeleteRoleExtensionsLinksAsync(this.Storage, RoleName);
            switch (deleted)
            {
                case true:
                    return StatusCode((int)HttpStatusCode.NoContent);
                case false:
                    return StatusCode((int)HttpStatusCode.BadRequest, "Link not deleted, the role is the last Admin grant to SoftinuxBase.Security extension");
                default:
                    return StatusCode((int)HttpStatusCode.BadRequest, "Role or link not found");
            }
        }

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Delete the records linking a role to any extension, then delete role record if possible (including links to users).
        /// </summary>
        /// <param name="roleNameList_">Comma-separated names of roles to delete.</param>
        /// <returns>Status code 200 (ok), or 400 with an error message.</returns>
        [HttpDelete]
        [ActionName("DeleteRole")]
        [Route("administration/delete-role/{roleNameList_}")]
        [HasPermission(typeof(Permissions.Enums.Permissions), (short)Permissions.Enums.Permissions.DeleteRoles)]
        public async Task<IActionResult> DeleteRoleAsync(string roleNameList_)
        {
            var errors = new List<string>();

            foreach (var role in roleNameList_.Split(new[] { ',' }))
            {
                var error = await DeleteRole.DeleteRoleAndAllLinksAsync(this.Storage, role);
                if (error != null)
                {
                    errors.Add(error);
                }
            }

            if (errors.Any())
            {
                return StatusCode((int)HttpStatusCode.BadRequest, errors);
            }

            return StatusCode((int)HttpStatusCode.OK);
        }

        #endregion

    }
}