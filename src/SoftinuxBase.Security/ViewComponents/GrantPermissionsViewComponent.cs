// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Barebone.ViewComponents;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.Security.ViewModels.Permissions;

namespace SoftinuxBase.Security.ViewComponents
{
    public class GrantPermissionsViewComponent : ViewComponentBase
    {
        private readonly IAspNetRolesManager _rolesManager;

        public GrantPermissionsViewComponent(IStorage storage_, IAspNetRolesManager rolesManager_) : base(storage_)
        {
            _rolesManager = rolesManager_;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            // Create a dictionary with all roles for injecting as json into grant permission page
            Dictionary<string, IdentityRole<string>> rolesList = new Dictionary<string, IdentityRole<string>>();

            // Create a dictionary with role id and name, since we will use role name in GrantViewModel
            // and we have role id in RolePermission table.
            Dictionary<string, string> roleNameByRoleId = new Dictionary<string, string>();

            foreach (var role in _rolesManager.Roles)
            {
                roleNameByRoleId.Add(role.Id, role.Name);
                rolesList.Add(role.Id, role);
            }

            ViewBag.RolesList = rolesList;

            GrantViewModel model = ReadGrants.ReadAll(Storage);
            return Task.FromResult<IViewComponentResult>(View("_List_Roles_Extensions", model));
        }
    }
}