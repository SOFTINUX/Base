// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.Extensions.Logging;

namespace SoftinuxBase.Barebone.ViewModels.Shared.MenuItem
{
    public class MenuItemViewModelFactory : ViewModelFactoryBase
    {
        public MenuItemViewModelFactory(IStorage storage_, ILoggerFactory loggerFactory_)
            : base(storage_, loggerFactory_)
        {
        }

        public MenuItemViewModel Create(Infrastructure.MenuItem menuItem_)
        {
            return new MenuItemViewModel()
            {
                Url = menuItem_.Url,
                Name = menuItem_.Name,
                Position = menuItem_.Position,
                FontAwesoneIconType = menuItem_.FontAwesomeIconType.ToString(),
                FontAwesomeIconClass = menuItem_.FontAwesomeIconClass
            };
        }
    }
}