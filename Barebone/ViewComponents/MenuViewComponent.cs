using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Barebone.ViewModels.Shared.Menu;

namespace Barebone.ViewComponents
{
    public class MenuViewComponent : ViewComponentBase
    {

        public MenuViewComponent(IStorage storage)
            : base(storage)
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new MenuViewModelFactory(this).Create());
        }
    }
}