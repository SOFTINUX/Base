// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Security.Common.Attributes;
using Security.Common.Enums;
using Security.ViewModels.Permissions;
using ControllerBase = Infrastructure.ControllerBase;

namespace Security.Controllers
{
    [PermissionRequirement(Permission.Admin, "Security")]
    public class GrantPermissionsController : ControllerBase
    {
        public GrantPermissionsController(IStorage storage_) : base(storage_)
        {

        }

         // GET
        [Route("administration/grantpermissions")]
        public IActionResult Index()
        {
            GlobalGrantViewModel model = new GlobalGrantViewModel();

            // sample very simple hardcoded data - TODO read data from database
            model.RolePermissions.Add("Admin", new List<Permission>
            {Permission.Admin, Permission.Write, Permission.Read});

            model.RolePermissions.Add("User", new List<Permission>
            {Permission.Write, Permission.Read});

            model.RolePermissions.Add("Anonymous", new List<Permission>
            {Permission.Read});

            return View(model);
        }
    }
}