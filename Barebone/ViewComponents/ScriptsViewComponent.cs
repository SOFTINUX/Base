using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Barebone.ViewModels.Shared.Scripts;
using Microsoft.Extensions.Logging;

namespace Barebone.ViewComponents
{
    public class ScriptsViewComponent : ViewComponentBase
    {
        public ScriptsViewComponent(ILoggerFactory loggerFactory_) : base(loggerFactory_)
        {
        }
        
        public Task<IViewComponentResult> InvokeAsync()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ScriptsViewModel model = new ScriptsViewModelFactory().Create();
            watch.Stop();
            LoggerFactory.CreateLogger<ScriptsViewComponent>().LogInformation("Time to build scripts list by ScriptsViewModelFactory: " + watch.ElapsedMilliseconds + " ms");
            return Task.FromResult<IViewComponentResult>(View(model));
        }


    }
}