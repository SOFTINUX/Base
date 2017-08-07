using System.Collections.Generic;
using System.Linq;
using ExtCore.Infrastructure;
using Infrastructure;
using Barebone.ViewModels.Shared.MenuItem;
using Barebone.ViewModels.Shared.MenuGroup;

namespace Barebone.ViewModels.Shared.Menu
{
    public class MenuViewModelFactory : ViewModelFactoryBase
    {
        /* public MenuViewModel Create()
        {
            List<Infrastructure.MenuItem> menuItems = new List<Infrastructure.MenuItem>();

            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
                menuItems.AddRange(extensionMetadata.MenuItems);

            return new MenuViewModel()
            {
                MenuItems = menuItems.OrderBy(mi => mi.Position).Select(mi => new MenuitemViewModelFactory().Create(mi))
            };
        } */

        public MenuViewModelFactory(IRequestHandler requestHandler)
            : base(requestHandler)
        {
        }

        public MenuViewModel Create()
        {
            List<MenuGroupViewModel> menuGroupViewModels = new List<MenuGroupViewModel>();

            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
                if (extensionMetadata.MenuGroups != null)
                {
                   foreach (Infrastructure.MenuGroup menuGroup in extensionMetadata.MenuGroups)
                   {
                       List<MenuItemViewModel> menuItemViewModels = new List<MenuItemViewModel>();

                       // TODO: here add claims verification for menu items

                       MenuGroupViewModel menuGroupViewModel = this.GetMenuGroup(menuGroupViewModels, menuGroup);

                       if (menuGroupViewModel.MenuItems != null)
                        menuItemViewModels.AddRange(menuGroupViewModel.MenuItems);

                        menuGroupViewModel.MenuItems = menuItemViewModels.OrderBy(mi => mi.Position);
                   }

                }
            }

            return new MenuViewModel()
            {
                MenuGroups = menuGroupViewModels.Where(mg => mg.MenuItems.Count() != 0).OrderBy(mg => mg.Position)
            };
        }

        private MenuGroupViewModel GetMenuGroup(List<MenuGroupViewModel> menuGroupViewModels_, Infrastructure.MenuGroup menuGroup_)
        {
            MenuGroupViewModel menuGroupViewModel = menuGroupViewModels_.FirstOrDefault(mg => mg.Name == menuGroup_.Name);

            if (menuGroupViewModel == null)
            {
                menuGroupViewModel = new MenuGroupViewModelFactory(this.RequestHandler).Create(menuGroup_);
                menuGroupViewModels_.Add(menuGroupViewModel);
            }
            else
            {
                // TODO ajouter les items issus de menuGroup_ à menuGroupViewModel

                MenuGroupViewModel tempViewModel = new MenuGroupViewModelFactory(this.RequestHandler).Create(menuGroup_);
                //menuGroupViewModel.AddMenuItems(tempViewModel.MenuItems);
            }

            return menuGroupViewModel;
        }
    }
}