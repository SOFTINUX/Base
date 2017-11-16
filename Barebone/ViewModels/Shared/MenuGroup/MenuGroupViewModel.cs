// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Barebone.ViewModels.Shared.MenuItem;

namespace Barebone.ViewModels.Shared.MenuGroup
{
    public class MenuGroupViewModel
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public IEnumerable<MenuItemViewModel> MenuItems { get; set; }

    }
}