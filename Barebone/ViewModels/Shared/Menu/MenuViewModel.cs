using System.Collections.Generic;
using Barebone.ViewModels.Shared.MenuItem;

namespace Barebone.ViewModels.Shared.Menu
{
    public class MenuViewModel
    {
        public IEnumerable<MenuItemViewModel> MenuItems {get; set;}
    }
}