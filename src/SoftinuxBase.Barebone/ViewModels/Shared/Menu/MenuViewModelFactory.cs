// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Infrastructure;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Barebone.ViewModels.Shared.MenuGroup;
using SoftinuxBase.Barebone.ViewModels.Shared.MenuItem;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Barebone.ViewModels.Shared.Menu
{
    public class MenuViewModelFactory : ViewModelFactoryBase
    {
        public MenuViewModelFactory(IRequestHandler requestHandler_, ILoggerFactory loggerFactory_)
            : base(requestHandler_, loggerFactory_)
        {
        }

        public MenuViewModel Create()
        {
            Dictionary<string, MenuGroupViewModel> menuGroupViewModels = new Dictionary<string, MenuGroupViewModel>();
            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
            {
#if DEBUG
                Logger.LogInformation($"Looking for menu groups for extension metadata {extensionMetadata.GetType().FullName}");
#endif

                if (extensionMetadata.MenuGroups == null)
                    continue;

                foreach (Infrastructure.MenuGroup menuGroup in extensionMetadata.MenuGroups)
                {
                    MenuGroupViewModel menuGroupViewModel = FindOrCreateMenuGroup(RequestHandler, menuGroupViewModels, menuGroup, Logger);

                    // Take existing items
                    List<MenuItemViewModel> menuItemViewModels = menuGroupViewModel.MenuItems;

                    if (menuGroup.MenuItems == null)
                    {
                        continue;
                    }

                    foreach (Infrastructure.MenuItem menuItem in menuGroup.MenuItems)
                    {

                        // TODO: here add claims verification for menu items
                        menuItemViewModels.Add(new MenuItemViewModelFactory(RequestHandler).Create(menuItem));
                    }

                    // Set all the ordered items back to menu group
                    menuGroupViewModel.MenuItems = menuItemViewModels.OrderBy(mi_ => mi_.Position).ToList();
                }
            }

            return new MenuViewModel()
            {
                // If two menu groups have the same position, they're ordered alphabetically
                // Don't add if no menu items
                MenuGroups = menuGroupViewModels.Values.Where(m_ => m_.MenuItems.Any()).OrderBy(m_ => m_.Position).ThenBy(m_ => m_.Name)
            };
        }

        /// <summary>
        /// Finds the MenuGroupViewModel in menuGroupViewModels_ or creates and returns it.
        /// </summary>
        /// <param name="requestHandler_"></param>
        /// <param name="menuGroupViewModels_"></param>
        /// <param name="menuGroup_"></param>
        /// <param name="logger_"></param>
        /// <returns></returns>
        private static MenuGroupViewModel FindOrCreateMenuGroup(IRequestHandler requestHandler_, IDictionary<string, MenuGroupViewModel> menuGroupViewModels_,
            Infrastructure.MenuGroup menuGroup_, ILogger logger_)
        {
            menuGroupViewModels_.TryGetValue(menuGroup_.Name, out var menuGroupViewModel);

            if (menuGroupViewModel == null)
            {
#if DEBUG
                logger_.LogInformation($"Menu group {menuGroup_.Name} found for first time, position {menuGroup_.Position}");
#endif
                menuGroupViewModel = new MenuGroupViewModelFactory(requestHandler_).Create(menuGroup_);
                menuGroupViewModels_.Add(menuGroupViewModel.Name, menuGroupViewModel);
            }
            else
            {
#if DEBUG
                logger_.LogInformation($"Menu group {menuGroup_.Name} already exists with position {menuGroup_.Position}");
#endif

                // If menu group already exist, the position and Font Awesome will be the one of the lowest position menu group
                if (menuGroup_.Position >= menuGroupViewModel.Position) return menuGroupViewModel;
#if DEBUG
                logger_.LogInformation($"Menu group {menuGroup_.Name} found with lower position {menuGroup_.Name}");
#endif
                menuGroupViewModel.FontAwesomeClass = menuGroup_.FontAwesomeClass;
                menuGroupViewModel.Position = menuGroup_.Position;
            }

            return menuGroupViewModel;
        }
    }
}