// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Barebone.ViewModels.Shared.StyleSheets;

namespace SoftinuxBase.Barebone.ViewComponents
{
    public class StyleSheetsViewComponent : ViewComponentBase
    {
        private readonly ILogger _logger;

        public StyleSheetsViewComponent(ILoggerFactory loggerFactory_) : base(loggerFactory_)
        {
            _logger = LoggerFactory.CreateLogger(GetType().FullName);
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            StyleSheetsViewModel model = new StyleSheetsViewModelFactory().Create();
            watch.Stop();
            _logger.LogInformation("Time to build stylesheets list by StyleSheetsViewModelFactory: " + watch.ElapsedMilliseconds + " ms");
            return Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}