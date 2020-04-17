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
        /// <param name="name_">set menu group display name.</param>
        /// <param name="position_">set position of menu group.</param>
        /// <param name="menuItems_">list of menu items to add to this menu group.</param>
        /// <param name="fontAwesomeIconType_">type for font awesome icon.</param>
        /// <param name="fontAwesomeClass_">icon linked to menu group.</param>
        public MenuGroup(string name_, uint position_, IEnumerable<MenuItem> menuItems_, FontAwesomeIcon.IconType fontAwesomeIconType_ = FontAwesomeIcon.IconType.Far, string fontAwesomeClass_ = "fa-circle")
        {
            Name = name_;
            Position = position_;
            MenuItems = menuItems_;
            FontAwesomeIconType = fontAwesomeIconType_;
            FontAwesomeIconClass = fontAwesomeClass_;
        }

        /// <summary>
        /// Gets menu group display name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets menu group position.
        /// </summary>
        public uint Position { get; }

        /// <summary>
        /// Gets the fa-xxx class to render the associated icon.
        /// </summary>
        public string FontAwesomeIconClass { get; }

        /// <summary>
        /// Gets the icon type for fontawesome.
        /// </summary>
        public FontAwesomeIcon.IconType FontAwesomeIconType { get; }

        /// <summary>
        /// Gets children menu items.
        /// </summary>
        public IEnumerable<MenuItem> MenuItems { get; }
    }
}