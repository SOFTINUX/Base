// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Diagnostics;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SoftinuxBase.Barebone.ViewModels.Barebone;
using ControllerBase = SoftinuxBase.Infrastructure.ControllerBase;

namespace SoftinuxBase.Barebone.Controllers
{
    public class BareboneController : Infrastructure.ControllerBase
    {
        private readonly string corporateName, corporateLogo;

        public BareboneController(IStorage storage_, IConfiguration configuration_) : base(storage_)
        {
            corporateName = configuration_["Corporate:Name"];
            corporateLogo = configuration_["Corporate:BrandLogo"];
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.CorporateName = corporateName;
            ViewBag.CorporateLogo = corporateLogo;
            return View(new IndexViewModelFactory().Create());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult Error()
        {
            return View(new Barebone.ViewModels.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}