using System.Collections.Generic;
using Barebone.ViewModels.Shared.MenuItem;

namespace Barebone.ViewModels.Shared.MenuGroup
{
    public class MenuGroupViewModel
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public IEnumerable<MenuItemViewModel> MenuItems { get; set; }

    }
}