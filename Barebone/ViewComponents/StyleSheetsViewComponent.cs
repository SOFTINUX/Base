using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Barebone.ViewModels.Shared.StyleSheet;

namespace Barebone.ViewComponents
{
    public class StyleSheetsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new StyleSheetsViewModelFactory().Create());
        }
    }
}