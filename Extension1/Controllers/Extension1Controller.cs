// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.


using Extension1.ViewModels.Extension1;
using Microsoft.AspNetCore.Mvc;
using Security.Common;

namespace Extension1.Controllers
{
    public class Extension1Controller : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexViexModelFactory().Create());
        }

        [Security.Common.Attributes.Authorize("Admin", "Extension1", PolicyUtil.AccessLevel.RW)]
        public ActionResult Admin()
        {
            return View();
        }
    }
}