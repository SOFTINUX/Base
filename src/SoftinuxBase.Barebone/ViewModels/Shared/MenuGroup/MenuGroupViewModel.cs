// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using SoftinuxBase.Barebone.ViewModels.Shared.MenuItem;

namespace SoftinuxBase.Barebone.ViewModels.Shared.MenuGroup
{
    public class MenuGroupViewModel
    {
        public string Name { get; set; }
        public uint Position { get; set; }
        public string FontAwesomeIconType { get; set; }
        public string FontAwesomeIconClass { get; set; }
        public List<MenuItemViewModel> MenuItems { get; set; }
    }
}