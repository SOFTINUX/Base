// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using Extension2.ViewModels.Extension2;
using Microsoft.AspNetCore.Mvc;

namespace Extension2.Controllers
{
    public class Extension2Controller : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexViexModelFactory().Create());
        }
    }
}