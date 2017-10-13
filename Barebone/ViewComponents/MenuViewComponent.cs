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
        public async Task<IViewComponentResult> InvokeAsync()
        {
            MenuViewModelFactory factory = new MenuViewModelFactory(this);
            
            Stopwatch watch = new Stopwatch();
            watch.Start();
            MenuViewModel menu = await factory.CreateAsync();
            watch.Stop();
            LoggerFactory.CreateLogger<MenuViewComponent>().LogInformation("Time to build menu content by MenuViewModelFactory: " + watch.ElapsedMilliseconds + " ms");
            return View(menu);
        }
    }
}