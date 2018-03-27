// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.


using Chinook.ViewModels.Chinook;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Security.Common.Attributes;
using Security.Common.Enums;

namespace Chinook.Controllers
{
    public class ChinookController : Controller
    {
        private readonly IStorage _storage;

        public ActionResult Index()
        {
            return View(new IndexViexModelFactory().Create());
        }

        [PermissionRequirement(Permission.Admin, "Chinook")]
        public ActionResult Admin()
        {
            return View();
        }

        [AnyPermissionRequirement(new []{Permission.Write, Permission.Admin}, new[]{"Security", "Security"})]
        public ActionResult Protected()
        {
            return View();
        }

        [PermissionRequirement(Permission.Admin, "Chinook")]
        public ActionResult Init()
        {
            return View("Protected");
        }
    }
}