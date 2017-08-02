using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Barebone.ViewModels.Shared.Scripts;

namespace Barebone.ViewComponents
{
    public class ScriptsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new ScriptsViewModelFactory().Create());
        }
    }
}