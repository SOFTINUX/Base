// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Security.Common.Attributes;
using SoftinuxBase.Security.Common.Enums;
using ControllerBase = SoftinuxBase.Infrastructure.ControllerBase;

namespace SoftinuxBase.Security.Controllers
{
    [PermissionRequirement(Permission.Admin)]
    public class AdministrationController : Infrastructure.ControllerBase
    {
        private readonly ILogger _logger;

        public AdministrationController(IStorage storage_, ILoggerFactory loggerFactory_) : base(storage_, loggerFactory_)
        {
            _logger = LoggerFactory.CreateLogger(GetType().FullName);
            _logger.LogInformation("oups");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View("Administration"));
        }
    }
}