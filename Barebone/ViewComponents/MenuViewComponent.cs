// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Barebone.ViewModels.Shared.Menu;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.Logging;

namespace Barebone.ViewComponents
{
    public class MenuViewComponent : ViewComponentBase
    {
        public MenuViewComponent(IStorage storage_, ILoggerFactory loggerFactory_) : base(storage_, loggerFactory_) { }

        /// <summary>
        /// Asynchronously builds menu.
        /// </summary>
        /// <returns></returns>
        public Task<IViewComponentResult> InvokeAsync()
        {
            MenuViewModelFactory factory = new MenuViewModelFactory(this);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            MenuViewModel menu = factory.Create();
            watch.Stop();
            LoggerFactory.CreateLogger<MenuViewComponent>().LogInformation("Time to build menu content by MenuViewModelFactory: " + watch.ElapsedMilliseconds + " ms");
            return Task.FromResult<IViewComponentResult>(View(menu));
        }
    }
}