// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Diagnostics;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Barebone.ViewModels.Shared.Menu;

namespace SoftinuxBase.Barebone.ViewComponents
{
    public class MenuViewComponent : ViewComponentBase
    {
        private readonly ILogger _logger;

        public MenuViewComponent(IStorage storage_, ILoggerFactory loggerFactory_) : base(storage_, loggerFactory_)
        {
            _logger = LoggerFactory.CreateLogger(GetType().FullName);
        }

        /// <summary>
        /// Asynchronously builds menu.
        /// </summary>
        /// <returns>IViewComponentResult object.</returns>
        public Task<IViewComponentResult> InvokeAsync()
        {
            MenuViewModelFactory factory = new MenuViewModelFactory(Storage, LoggerFactory);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            MenuViewModel menu = factory.Create();
            watch.Stop();
            _logger.LogInformation("Time to build menu content by MenuViewModelFactory: " + watch.ElapsedMilliseconds + " ms");
            return Task.FromResult<IViewComponentResult>(View(menu));
        }
    }
}