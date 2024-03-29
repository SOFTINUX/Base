// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Diagnostics;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SoftinuxBase.Barebone.ViewModels.Barebone;

namespace SoftinuxBase.Barebone.Controllers
{
    public class BareboneController : Infrastructure.ControllerBase
    {
        private readonly string _corporateName;
        private readonly string _corporateLogo;

        /// <summary>
        /// Initializes a new instance of the <see cref="BareboneController"/> class.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="configuration_">Application configuiration object.</param>
        public BareboneController(IStorage storage_, IConfiguration configuration_) : base(storage_)
        {
            _corporateName = configuration_["Corporate:Name"];
            _corporateLogo = configuration_["Corporate:BrandLogo"];
        }

        /// <summary>
        /// Index controller.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            ViewBag.CorporateName = _corporateName;
            ViewBag.CorporateLogo = _corporateLogo;
            return await Task.Run(() => View(new IndexViewModelFactory().Create()));
        }

        /// <summary>
        /// Partial view for errors.
        /// </summary>
        /// <returns>Error view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        [ActionName("Error")]
        public async Task<IActionResult> ErrorAsync()
        {
            return await Task.Run(() => View(new ViewModels.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
        }
    }
}