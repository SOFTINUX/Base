// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

namespace Infrastructure
{
    public class MenuGroup
    {
        public string Name {get; set;}
        public int Position {get;}

        public IEnumerable<MenuItem> MenuItems {get;}

        public MenuGroup(string name_, int position_, IEnumerable<MenuItem> menuItems_)
        {
            Name = name_;
            Position = position_;
            MenuItems = menuItems_;
        }
    }
}