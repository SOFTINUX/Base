// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Barebone.ViewComponents;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Security.ViewComponents
{
    public class SelectOptionsListRolesViewComponent : ViewComponentBase
    {
        private readonly IAspNetRolesManager _rolesManager;

        public SelectOptionsListRolesViewComponent(IStorage storage_, IAspNetRolesManager rolesManager_) : base(storage_)
        {
            _rolesManager = rolesManager_;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            // TODO rewrite for new permissions

            // for now empty
            HashSet<string> listPermissionsRoleId = new HashSet<string>();

            // HashSet<string> listPermissionsRoleId = _storage.GetRepository<RolePermissionRepository>().All().Select(item_ => item_.RoleId).ToHashSet();
            return Task.FromResult<IViewComponentResult>(View("_SelectOptionsListRoles", (_rolesManager, listPermissionsRoleId)));
        }
    }
}