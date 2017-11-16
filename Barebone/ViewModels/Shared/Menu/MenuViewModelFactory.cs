// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Infrastructure;
using Infrastructure;
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
            List<MenuGroupViewModel> menuGroupViewModels = new List<MenuGroupViewModel>();
            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
                if (extensionMetadata.MenuGroups == null) continue;

                foreach (Infrastructure.MenuGroup menuGroup in extensionMetadata.MenuGroups)
                {
                    List<MenuItemViewModel> menuItemViewModels = new List<MenuItemViewModel>();

                    foreach (Infrastructure.MenuItem menuItem in menuGroup.MenuItems)
                        // TODO: here add claims verification for menu items
                        menuItemViewModels.Add(
                            new MenuItemViewModelFactory(RequestHandler).Create(menuItem));

                    MenuGroupViewModel menuGroupViewModel = FindOrCreateMenuGroup(RequestHandler, menuGroupViewModels, menuGroup);

                    if (menuGroupViewModel.MenuItems != null)
                        menuItemViewModels.AddRange(menuGroupViewModel.MenuItems);

                    menuGroupViewModel.MenuItems = menuItemViewModels.OrderBy(mi => mi.Position);
                }
            }
            return new MenuViewModel()
            {
                MenuGroups = menuGroupViewModels
            };
        }

        /// <summary>
        /// Finds the MenuGroupViewModel in menuGroupViewModels_ or creates and returns it.
        /// </summary>
        /// <param name="requestHandler_"></param>
        /// <param name="menuGroupViewModels_"></param>
        /// <param name="menuGroup_"></param>
        /// <returns></returns>
        private static MenuGroupViewModel FindOrCreateMenuGroup(IRequestHandler requestHandler_, List<MenuGroupViewModel> menuGroupViewModels_,
            Infrastructure.MenuGroup menuGroup_)
        {
            MenuGroupViewModel menuGroupViewModel =
                menuGroupViewModels_.FirstOrDefault(mg => mg.Name == menuGroup_.Name);

            if (menuGroupViewModel != null)
                return menuGroupViewModel;

            menuGroupViewModel = new MenuGroupViewModelFactory(requestHandler_).Create(menuGroup_);
            menuGroupViewModels_.Add(menuGroupViewModel);

            return menuGroupViewModel;
        }
    }
}