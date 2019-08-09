// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Barebone.ViewModels.Shared.Scripts;

namespace SoftinuxBase.Barebone.ViewComponents
{
    public class ScriptsViewComponent : ViewComponentBase
    {
        private readonly ILogger _logger;

        public ScriptsViewComponent(ILoggerFactory loggerFactory_) : base(loggerFactory_)
        {
            _logger = LoggerFactory.CreateLogger(GetType().FullName);
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ScriptsViewModel model = new ScriptsViewModelFactory().Create();
            watch.Stop();
            _logger.LogInformation("Time to build scripts list by ScriptsViewModelFactory: " + watch.ElapsedMilliseconds + " ms");
            return Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}