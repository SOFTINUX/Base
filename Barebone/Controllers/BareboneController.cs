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
        private readonly IDatabaseInitializer _databaseInitializer;
        private readonly string corporateName, corporateLogo;

        public BareboneController(IStorage storage_, IDatabaseInitializer databaseInitializer_, IConfiguration configuration_) : base(storage_)
        {
            corporateName = configuration_["Corporate:Name"];
            corporateLogo = configuration_["Corporate:BrandLogo"];
            _databaseInitializer = databaseInitializer_;
        }

        public ActionResult Index()
        {
            ViewBag.CorporateName = corporateName;
            ViewBag.CorporateLogo = corporateLogo;
            _databaseInitializer.CheckAndInitialize(this);
            return View(new IndexViewModelFactory().Create());
        }

    }
}