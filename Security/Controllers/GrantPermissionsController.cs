// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using Security.ViewModels.Permissions;
using ControllerBase = Infrastructure.ControllerBase;

namespace Security.Controllers
{
    [PermissionRequirement("Admin", "Security")]
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
            model.RolePermissions.Add("Admin", new List<Infrastructure.Enums.Permission>
            {Infrastructure.Enums.Permission.Admin, Infrastructure.Enums.Permission.Write, Infrastructure.Enums.Permission.Read});

            model.RolePermissions.Add("Write", new List<Infrastructure.Enums.Permission>
            {Infrastructure.Enums.Permission.Write, Infrastructure.Enums.Permission.Read});

            model.RolePermissions.Add("Read", new List<Infrastructure.Enums.Permission>
            {Infrastructure.Enums.Permission.Read});

            return View(model);
        }
    }
}