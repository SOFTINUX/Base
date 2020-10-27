// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Barebone.ViewComponents;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.Abstractions;

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
            HashSet<string> listNamesOfRolesWithPermissions = Storage.GetRepository<IRoleToPermissionsRepository>().All().Where(rp_ => rp_.PermissionsForRole.Any()).Select(rp_ => rp_.RoleName).ToHashSet();
            return Task.FromResult<IViewComponentResult>(View("_SelectOptionsListRoles", (_rolesManager, listNamesOfRolesWithPermissions)));
        }
    }
}