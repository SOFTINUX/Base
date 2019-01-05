// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using SoftinuxBase.Barebone.ViewModels.Shared.MenuItem;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Barebone.ViewModels.Shared.MenuGroup
{
    public class MenuGroupViewModelFactory : ViewModelFactoryBase
    {

        public MenuGroupViewModelFactory(IRequestHandler requestHandler_)
            : base(requestHandler_)
        {
        }

        public MenuGroupViewModel Create(Infrastructure.MenuGroup menuGroup_)
        {
            return new MenuGroupViewModel()
            {
                Name = menuGroup_.Name,
                Position = menuGroup_.Position,
                FontAwesomeClass = menuGroup_.FontAwesomeClass,
                MenuItems = new List<MenuItemViewModel>()
            };
        }
    }
}