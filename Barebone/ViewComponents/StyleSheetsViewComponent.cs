using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Barebone.ViewModels.Shared.StyleSheet;
using Barebone.ViewModels.Shared.StyleSheets;
using Microsoft.Extensions.Logging;

namespace Barebone.ViewComponents
{
    public class StyleSheetsViewComponent : ViewComponentBase
    {
        public StyleSheetsViewComponent(ILoggerFactory loggerFactory_) : base(loggerFactory_)
        {
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            StyleSheetsViewModel model = new StyleSheetsViewModelFactory().Create();
            watch.Stop();
            LoggerFactory.CreateLogger<StyleSheetsViewComponent>().LogInformation("Time to build stylesheets list by StyleSheetsViewModelFactory: " + watch.ElapsedMilliseconds + " ms");
            return Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}