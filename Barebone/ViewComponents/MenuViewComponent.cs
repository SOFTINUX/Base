using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Barebone.ViewModels.Shared.Menu;
using ExtCore.Data.Abstractions;

namespace Barebone.ViewComponents
{
    public class MenuViewComponent : ViewComponentBase
    {
        public MenuViewComponent(IStorage storage_) : base(storage_) { }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            MenuViewModelFactory factory = new MenuViewModelFactory(this);
            
            MenuViewModel menu = await factory.CreateAsync();
            return View(menu);
        }
    }
}