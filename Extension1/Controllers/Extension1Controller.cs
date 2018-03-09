// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.


using System.Net;
using Extension1.ViewModels.Extension1;
using Infrastructure.Attributes;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Extension1.Controllers
{
    public class Extension1Controller : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexViexModelFactory().Create());
        }

        [PermissionRequirement(Infrastructure.Enums.Permission.Admin, "Extension1")]
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