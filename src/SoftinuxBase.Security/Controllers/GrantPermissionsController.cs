// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        [ActionName("Index")]
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
        public async Task<IActionResult> ReadRoleAsync(string roleId_)
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

        /// <summary>
        /// Return updated roles list.
        /// </summary>
        /// <returns>view component.</returns>
        [Route("administration/edit-role-tab")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
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
        public IActionResult RefreshBulkDeleteTab()
        {
            return ViewComponent("BulkDeleteRoles");
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
        [ActionName("SaveNewRoleAndItsPermissions")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SaveNewRoleAndItsPermissionsAsync([FromBody] SaveNewRoleAndGrantsViewModel model_)
        {
            string error = await CreateRoleAndGrants.CheckAndSaveNewRoleAndGrantsAsync(Storage, _roleManager, model_);
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
        [ActionName("UpdateRolePermission")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRolePermissionAsync([FromBody] UpdateRolePermissionViewModel model_)
        {
            string roleId = (await _roleManager.FindByNameAsync(model_.RoleName)).Id;
            IRolePermissionRepository repo = Storage.GetRepository<IRolePermissionRepository>();
            repo.Delete(roleId, model_.Extension);

            if (Enum.TryParse<Permission>(model_.PermissionValue, true, out var permissionEnumValue))
            {
                var permissionEntity = Storage.GetRepository<IPermissionRepository>().Find(permissionEnumValue);
                repo.Create(new RolePermission { RoleId = roleId, PermissionId = permissionEntity.Id, Extension = model_.Extension });
            }

            await Storage.SaveAsync();
            return StatusCode((int)HttpStatusCode.OK);
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
        public async Task<IActionResult> UpdateRoleAndItsPermissionsAsync([FromBody] UpdateRoleAndGrantsViewModel model_)
        {
            string error = await UpdateRoleAndGrants.CheckAndUpdateRoleAndGrantsAsync(Storage, _roleManager, model_);
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
        [ActionName("DeleteRoleExtensionLink")]
        [Route("administration/delete-role-extension/{RoleName}/{ExtensionName}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1313", Justification = "Ignore camelcase parameters")]
        public async Task<IActionResult> DeleteRoleExtensionLinkAsync(string RoleName, string ExtensionName)
        {
            bool? deleted = await DeleteRole.DeleteRoleExtensionLinkAsync(this.Storage, _roleManager, ExtensionName, RoleName);
            if (deleted == true)
            {
                return StatusCode((int)HttpStatusCode.NoContent);
            }

            if (deleted == false)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Link not deleted, the role is the last Admin grant to SoftinuxBase.Security extension");
            }

            return StatusCode((int)HttpStatusCode.BadRequest, "Role or link not found");
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
        public async Task<IActionResult> DeleteRoleAsync(string roleNameList_)
        {
            var errors = new List<string>();

            foreach (var role in roleNameList_.Split(new[] { ',' }))
            {
                var error = await DeleteRole.DeleteRoleAndAllLinksAsync(this.Storage, _roleManager, role);
                if (error != null)
                {
                    errors.Add(error);
                }
            }

            if (errors.Any())
            {
                return StatusCode((int)HttpStatusCode.BadRequest, errors);
            }

            return StatusCode((int) HttpStatusCode.OK);
        }

        #endregion

    }
}