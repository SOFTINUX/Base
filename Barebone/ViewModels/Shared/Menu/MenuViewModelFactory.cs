// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Infrastructure;
using Infrastructure.Interfaces;
using Barebone.ViewModels.Shared.MenuItem;
using Barebone.ViewModels.Shared.MenuGroup;

namespace Barebone.ViewModels.Shared.Menu
{
    public class MenuViewModelFactory : ViewModelFactoryBase
    {
        public MenuViewModelFactory(IRequestHandler requestHandler_)
            : base(requestHandler_)
        {
        }

        public MenuViewModel Create()
        {
            Dictionary<string, MenuGroupViewModel> menuGroupViewModels = new Dictionary<string, MenuGroupViewModel>();
            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
                if (extensionMetadata.MenuGroups == null) continue;

                foreach (Infrastructure.MenuGroup menuGroup in extensionMetadata.MenuGroups)
                {
                    MenuGroupViewModel menuGroupViewModel = FindOrCreateMenuGroup(RequestHandler, menuGroupViewModels, menuGroup);

                    // Take existing items
                    List<MenuItemViewModel> menuItemViewModels = menuGroupViewModel.MenuItems;

                    foreach (Infrastructure.MenuItem menuItem in menuGroup.MenuItems)
                        // TODO: here add claims verification for menu items
                        menuItemViewModels.Add(
                            new MenuItemViewModelFactory(RequestHandler).Create(menuItem));

                    // Set all the ordered items back to menu group
                    menuGroupViewModel.MenuItems = menuItemViewModels.OrderBy(mi => mi.Position).ToList();
                }
            }
            return new MenuViewModel()
            {
                // If two menu groups have the same position, they're ordered alphabetically
                MenuGroups = menuGroupViewModels.Values.OrderBy(m_ => m_.Position).ThenBy(m_ => m_.Name)
            };
        }

        /// <summary>
        /// Finds the MenuGroupViewModel in menuGroupViewModels_ or creates and returns it.
        /// </summary>
        /// <param name="requestHandler_"></param>
        /// <param name="menuGroupViewModels_"></param>
        /// <param name="menuGroup_"></param>
        /// <returns></returns>
        private static MenuGroupViewModel FindOrCreateMenuGroup(IRequestHandler requestHandler_, Dictionary<string, MenuGroupViewModel> menuGroupViewModels_,
            Infrastructure.MenuGroup menuGroup_)
        {
            MenuGroupViewModel menuGroupViewModel;
            menuGroupViewModels_.TryGetValue(menuGroup_.Name, out menuGroupViewModel);

            if (menuGroupViewModel == null)
            {
                menuGroupViewModel = new MenuGroupViewModelFactory(requestHandler_).Create(menuGroup_);
                menuGroupViewModels_.Add(menuGroupViewModel.Name, menuGroupViewModel);
            }
            else
            {
                // If menu group already exist, the Font Awesome will be the one of the lowest position menu group
                if(menuGroup_.Position < menuGroupViewModel.Position)
                    menuGroupViewModel.FontAwesomeClass = menuGroup_.FontAwesomeClass;
            }

            return menuGroupViewModel;
        }
    }
}