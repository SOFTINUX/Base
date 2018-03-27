// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.


using System.IO;
using System.Reflection;
using Chinook.ViewModels.Chinook;
using ExtCore.Data.Abstractions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Security.Common.Attributes;
using Security.Common.Enums;

namespace Chinook.Controllers
{
    public class ChinookController : Infrastructure.ControllerBase
    {
        public ChinookController(IStorage storage_) : base(storage_)
        {

        }

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
            string result = new SqlHelper(Storage).ExecuteSqlFile(
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Chinook_Sqlite_AutoIncrementPKs.sql"));
            this.ViewBag.InitDone = true;
            this.ViewBag.InitResult = result;
            return View("Protected");
        }
    }
}