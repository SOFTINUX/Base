// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Barebone.ViewModels.Barebone;
using ExtCore.Data.Abstractions;
using Infrastructure;
using ControllerBase = Infrastructure.ControllerBase;
using Microsoft.Extensions.Configuration;

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

    }
}