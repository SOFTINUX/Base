// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Barebone.ViewModels.Barebone;
using ExtCore.Data.Abstractions;
using ControllerBase = Infrastructure.ControllerBase;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Barebone.Controllers
{
    public class BareboneController : ControllerBase
    {
        private readonly string corporateName, corporateLogo;

        public BareboneController(IStorage storage_, IConfiguration configuration_) : base(storage_)
        {
            corporateName = configuration_["Corporate:Name"];
            corporateLogo = configuration_["Corporate:BrandLogo"];
        }

        public ActionResult Index()
        {
            ViewBag.CorporateName = corporateName;
            ViewBag.CorporateLogo = corporateLogo;
            return View(new IndexViewModelFactory().Create());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Barebone.ViewModels.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}