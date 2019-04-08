// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

namespace SoftinuxBase.Infrastructure
{
    public class MenuGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuGroup"/> class.
        /// </summary>
        /// <param name="name_">set menu name to display.</param>
        /// <param name="position_">set position of menu group.</param>
        /// <param name="menuItems_">list of menu items from enum.</param>
        /// <param name="fontAwesomeClass_">icon linked to menu group.</param>
        public MenuGroup(string name_, uint position_, IEnumerable<MenuItem> menuItems_, string fontAwesomeClass_ = "fa-bars")
        {
            Name = name_;
            Position = position_;
            MenuItems = menuItems_;
            FontAwesomeClass = fontAwesomeClass_;
        }

        /// <summary>
        /// Gets menu group display name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets menug group position.
        /// </summary>
        public uint Position { get; }

        /// <summary>
        /// Gets the fa-xxx class to render the associated icon.
        /// </summary>
        public string FontAwesomeClass { get; }

        /// <summary>
        /// Gets enum of menu items.
        /// </summary>
        public IEnumerable<MenuItem> MenuItems { get; }
    }
}