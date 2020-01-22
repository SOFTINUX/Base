// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Security.Common.Enums;
using SoftinuxBase.Security.FeatureAuthorize.PolicyCode;

namespace SoftinuxBase.Security.Controllers
{
    // [PermissionRequirement(Permission.Admin, Constants.SoftinuxBaseSecurity)]
    [HasPermission(typeof(Permissions), (short)Permissions.Admin)]
    public class AdministrationController : Infrastructure.ControllerBase
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministrationController"/> class.
        /// </summary>
        /// <param name="storage_">application storage instance.</param>
        /// <param name="loggerFactory_">application logger factory instance.</param>
        public AdministrationController(IStorage storage_, ILoggerFactory loggerFactory_) : base(storage_, loggerFactory_)
        {
            _logger = LoggerFactory?.CreateLogger(GetType().FullName);
            _logger?.LogInformation("oups");
        }

        /// <summary>
        /// Administration Index view.
        /// </summary>
        /// <returns>administration Index view.</returns>
        [HttpGet]
        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            return await Task.Run(() => View("Administration"));
        }
    }
}