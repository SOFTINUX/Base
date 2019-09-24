// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Barebone.ViewComponents;
using SoftinuxBase.Security.Data.EntityFramework;

namespace SoftinuxBase.Security.ViewComponents
{
    public class BulkDeleteRolesViewComponent : ViewComponentBase
    {
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly IStorage _storage;

        public BulkDeleteRolesViewComponent(IStorage storage_, RoleManager<IdentityRole<string>> roleManager_) : base(storage_)
        {
            _roleManager = roleManager_;
            _storage = storage_;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            HashSet<string> listPermissionsRoleId = _storage.GetRepository<RolePermissionRepository>().All().Select(item_ => item_.RoleId).ToHashSet();

            return Task.FromResult<IViewComponentResult>(View("_BulkDeleteRoles", (_roleManager, listPermissionsRoleId)));
        }
    }
}
