// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Barebone.ViewComponents;

namespace SoftinuxBase.Security.ViewComponents
{
    public class SelectOptionsListRolesViewComponent : ViewComponentBase
    {
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly IStorage _storage;

        public SelectOptionsListRolesViewComponent(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_) : base(storage_)
        {
            _roleManager = roleManager_;
            _storage = storage_;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            // TODO rewrite for new permissions
            // for now empty
            HashSet<string> listPermissionsRoleId = new HashSet<string>();
            // HashSet<string> listPermissionsRoleId = _storage.GetRepository<RolePermissionRepository>().All().Select(item_ => item_.RoleId).ToHashSet();

            return Task.FromResult<IViewComponentResult>(View("_SelectOptionsListRoles", (_roleManager, listPermissionsRoleId)));
        }
    }
}
