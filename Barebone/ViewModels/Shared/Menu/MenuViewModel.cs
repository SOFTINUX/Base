using System.Collections.Generic;
using Barebone.ViewModels.Shared.MenuItem;
using Barebone.ViewModels.Shared.MenuGroup;

namespace Barebone.ViewModels.Shared.Menu
{
    public class MenuViewModel
    {
        public IEnumerable<MenuGroupViewModel> MenuGroups {get; set;}
    }
}