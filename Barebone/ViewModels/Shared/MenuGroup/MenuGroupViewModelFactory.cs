using System.Collections.Generic;
using Barebone.ViewModels.Shared.MenuItem;

namespace Barebone.ViewModels.Shared.MenuGroup
{
    public class MenuGroupViewModelFactory : ViewModelFactoryBase
    {

        public MenuGroupViewModelFactory(IRequestHandler requestHandler)
            : base(requestHandler)
        {
        }

        public MenuGroupViewModel Create(Infrastructure.MenuGroup menuGroup_)
        {
            MenuGroupViewModel viewModel = new MenuGroupViewModel()
            {
                Name = menuGroup_.Name,
                Position = menuGroup_.Position
            };

            // Factories the menu items to menu item view models
            List<MenuItemViewModel> listMenuItemViewModel = new List<MenuItemViewModel>();
            MenuitemViewModelFactory menuItemFactory = new MenuitemViewModelFactory();
            foreach (Infrastructure.MenuItem menuItem in menuGroup_.MenuItems)
            {
                listMenuItemViewModel.Add(menuItemFactory.Create(menuItem));
            }
            viewModel.MenuItems = listMenuItemViewModel;
            return viewModel;
        }
    }
}