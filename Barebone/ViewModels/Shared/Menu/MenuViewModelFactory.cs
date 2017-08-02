using System.Collections.Generic;
using System.Linq;
using ExtCore.Infrastructure;
using Infrastructure;
using Barebone.ViewModels.Shared.MenuItem;

namespace Barebone.ViewModels.Shared.Menu
{
    public class MenuViewModelFactory
    {
        public MenuViewModel Create()
        {
            List<Infrastructure.MenuItem> menuItems = new List<Infrastructure.MenuItem>();

            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
                menuItems.AddRange(extensionMetadata.MenuItems);

            return new MenuViewModel()
            {
                MenuItems = menuItems.OrderBy(mi => mi.Position).Select(mi => new MenuitemViewModelFactory().Create(mi))
            };
        }
    }
}