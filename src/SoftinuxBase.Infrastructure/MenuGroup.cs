// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

namespace SoftinuxBase.Infrastructure
{
    public class MenuGroup
    {
        public MenuGroup(string name_, uint position_, IEnumerable<MenuItem> menuItems_, string fontAwesomeClass_ = "fa-bars")
        {
            Name = name_;
            Position = position_;
            MenuItems = menuItems_;
            FontAwesomeClass = fontAwesomeClass_;
        }

        public string Name { get; set; }

        public uint Position { get; }

        /// <summary>
        /// Gets the fa-xxx class to render the associated icon.
        /// </summary>
        public string FontAwesomeClass { get; }

        public IEnumerable<MenuItem> MenuItems { get; }
    }
}