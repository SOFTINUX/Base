// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Security.Common;
using Security.Common.Attributes;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.Extensions;
using Security.ViewModels.Permissions;
using ControllerBase = Infrastructure.ControllerBase;
using Permission = Security.Common.Enums.Permission;

namespace Security.Controllers
{
    [PermissionRequirement(Common.Enums.Permission.Admin)]
    public class GrantPermissionsController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<string>> _roleManager;

        public GrantPermissionsController(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_) : base(storage_)
        {
            _roleManager = roleManager_;
        }

         // GET
        [PermissionRequirement(Permission.Admin)]
        [Route("administration/grantpermissions")]
        public IActionResult Index()
        {
            // TODO move all this code to Tools.PermissionTools (new static class)
            GrantViewModel model = new GrantViewModel();

            // Create a dictionary with role id and name, since we will use role name in GrantViewModel
            // and we have role id in RolePermission table.
            Dictionary<string, string> roleNameByRoleId = new Dictionary<string, string>();
            var roles = _roleManager.Roles.ToList();
            model.RolesManager = roles;
            foreach(var role in roles)
            {
                roleNameByRoleId.Add(role.Id, role.Name);
            }

            // 1. Get all scopes from available extensions, create initial dictionaries
            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
                model.PermissionsByRoleAndScope.Add(extensionMetadata.GetScope(), new Dictionary<string, List<Common.Enums.Permission>>());
            }

            // 2. Read data from RolePermission table
            // Names of roles that have permissions attributed
            HashSet<string> rolesWithPerms = new HashSet<string>();

            // Read role/permission/extension settings
            List<RolePermission> allRp = Storage.GetRepository<IRolePermissionRepository>().All();
            foreach(RolePermission rp in allRp)
            {
                if(!model.PermissionsByRoleAndScope.ContainsKey(rp.Scope))
                {
                    // A database record related to a not loaded extension (scope). Ignore this.
                    continue;
                }
                string roleName = roleNameByRoleId.ContainsKey(rp.RoleId) ? roleNameByRoleId[rp.RoleId] : null;
                if(!model.PermissionsByRoleAndScope[rp.Scope].ContainsKey(roleName))
                    model.PermissionsByRoleAndScope[rp.Scope].Add(roleName, new List<Common.Enums.Permission>());
                // Format the list of Permission enum values according to DB enum value
                model.PermissionsByRoleAndScope[rp.Scope][roleName] = PermissionHelper.GetLowerOrEqual(PermissionHelper.FromId(rp.PermissionId));
                rolesWithPerms.Add(roleName);
            }

            // 3. Also read roles for which no permissions were set
            IList<string> roleNames = _roleManager.Roles.Select(r_ => r_.Name).ToList();
            foreach (string role in roleNames)
            {
                if(rolesWithPerms.Contains(role))
                    continue;
                foreach(string scope in model.PermissionsByRoleAndScope.Keys)
                {
                    model.PermissionsByRoleAndScope[scope].Add(role, new List<Common.Enums.Permission>());
                }
            }
            return View(model);
        }
        /// <summary>
        /// Update scoped role-permission.
        /// </summary>
        /// <param name="roleName_">Role name</param>
        /// <param name="permissionId_">New permission level to save</param>
        /// <param name="scope_">Scope</param>
        /// <returns>JSON with "true" when it succeeded</returns>
        // GET
        [PermissionRequirement(Permission.Admin)]
        [Route("administration/updaterolepermission")]
        public async Task<IActionResult> UpdateRole(string roleName_, string permissionId_, string scope_)
        {
            string roleId = (await _roleManager.FindByNameAsync(roleName_)).Id;
            IRolePermissionRepository repo = Storage.GetRepository<IRolePermissionRepository>();
            repo.Delete(roleId, scope_);
            if(!string.IsNullOrEmpty(permissionId_.ToLowerInvariant()))
                repo.Create(new RolePermission { RoleId = roleId, PermissionId = permissionId_.UppercaseFirst(), Scope = scope_ });
            Storage.Save();
            return new JsonResult(true);
        }

        /// <summary>
        /// Delete a link between a role and an extension.
        /// </summary>
        /// <param name="roleName_"></param>
        /// <param name="scope_"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteRoleLink(string roleName_, string scope_)
        {
            string roleId = (await _roleManager.FindByNameAsync(roleName_)).Id;
            IRolePermissionRepository repo = Storage.GetRepository<IRolePermissionRepository>();
            if (repo.FindBy(roleId, scope_) != null)
            {
                repo.Delete(roleId, scope_);
                Storage.Save();
                return new JsonResult(true);
            }
            return new JsonResult(false);
        }

        [HttpPost]
        public ObjectResult SaveNewRole(SaveNewRoleViewModel model_)
        {
            string error = Tools.CreateRole.CheckAndSaveNewRole(model_, _roleManager, Storage).Result;
            return StatusCode(string.IsNullOrEmpty(error) ? 201 : 400, error);
        }
    }
}