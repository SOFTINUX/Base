// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Barebone.ViewModels.Shared.MenuItem
{
    public class MenuItemViewModelFactory : ViewModelFactoryBase
    {
        public MenuItemViewModelFactory(IRequestHandler requestHandler_)
            : base(requestHandler_)
        {
        }
        public MenuItemViewModel Create(Infrastructure.MenuItem menuItem_)
        {
            return new MenuItemViewModel()
            {
                Url = menuItem_.Url,
                Name = menuItem_.Name,
                Position = menuItem_.Position,
                FontAwesomeClass = menuItem_.FontAwesomeClass
            };
        }
    }
}