// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Barebone.ViewModels.Shared.MenuItem;
using Infrastructure;

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
            return new MenuGroupViewModel()
            {
                Name = menuGroup_.Name,
                Position = menuGroup_.Position
            };
        }
    }
}