// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.


using Extension1.ViewModels.Extension1;
using Microsoft.AspNetCore.Mvc;
using Security.Common.Attributes;
using Security.Common.Enums;

namespace Extension1.Controllers
{
    public class Extension1Controller : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexViexModelFactory().Create());
        }

        [PermissionRequirement(Permission.Admin, "Extension1")]
        public ActionResult Admin()
        {
            return View();
        }

        [AnyPermissionRequirement(new []{Permission.Write, Permission.Admin}, new[]{"Security", "Security"})]
        public ActionResult Protected()
        {
            return View();
        }
    }
}