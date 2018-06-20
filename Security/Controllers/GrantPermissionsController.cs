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
            GrantViewModel model = new GrantViewModel();

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
                if(!model.PermissionsByRoleAndScope[rp.Scope].ContainsKey(rp.RoleId))
                    model.PermissionsByRoleAndScope[rp.Scope].Add(rp.RoleId, new List<Common.Enums.Permission>());
                // Format the list of Permission enum values according to DB enum value
                model.PermissionsByRoleAndScope[rp.Scope][rp.RoleId] = PermissionHelper.GetLowerOrEqual(PermissionHelper.FromId(rp.PermissionId));
                rolesWithPerms.Add(rp.RoleId);
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
        /// <param name="roleId_">Role</param>
        /// <param name="permissionId_">New permission level to save</param>
        /// <param name="scope_">Scope</param>
        /// <returns>JSON with "true" when it succeeded</returns>
        // GET
        [PermissionRequirement(Permission.Admin)]
        [Route("administration/updaterolepermission")]
        public IActionResult UpdateRole(string roleId_, string permissionId_, string scope_)
        {
            IRolePermissionRepository repo = Storage.GetRepository<IRolePermissionRepository>();
            repo.Delete(roleId_, scope_);
            if(!string.IsNullOrEmpty(permissionId_.ToLowerInvariant()))
                repo.Create(new RolePermission { RoleId = roleId_, PermissionId = UppercaseFirst(permissionId_.ToLowerInvariant()), Scope = scope_ });
            Storage.Save();
            return new JsonResult(true);
        }

        /// <summary>
        /// Delete a link between a role and an extension.
        /// </summary>
        /// <param name="roleId_"></param>
        /// <param name="scope_"></param>
        /// <returns></returns>
        public IActionResult DeleteRoleLink(string roleId_, string scope_)
        {
            IRolePermissionRepository repo = Storage.GetRepository<IRolePermissionRepository>();
            if (repo.FindBy(roleId_, scope_) != null)
            {
                repo.Delete(roleId_, scope_);
                Storage.Save();
                return new JsonResult(true);
            }
            return new JsonResult(false);
        }

        [HttpPost]
        public async Task<ObjectResult> SaveNewRole(SaveNewRoleViewModel model_)
        {
            if(!ModelState.IsValid)
            {
                return StatusCode(400, ModelState["Role"]);
            }
            // Convert the string to the enum
            var permissionEnum = Enum.Parse<Security.Common.Enums.Permission>(model_.Permission, true);

            // Save the Role
            IdentityRole<string> identityRole = new IdentityRole<string>
                {
                    Id = model_.Role,
                    Name = model_.Role
                };
            await _roleManager.CreateAsync(identityRole);

            // Save the role-extension-permission link
            IRolePermissionRepository repo = Storage.GetRepository<IRolePermissionRepository>();
            foreach(string extension in model_.Extensions)
            {
                repo.Create(new RolePermission{RoleId = model_.Role, PermissionId = permissionEnum.ToString(), Scope = extension});
            }
            return StatusCode(201, string.Empty);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s_"></param>
        /// <returns></returns>
        static string UppercaseFirst(string s_)
        {
            if (string.IsNullOrEmpty(s_))
            {
                return string.Empty;
            }
            char[] a = s_.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}