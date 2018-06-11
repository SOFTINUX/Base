// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Chinook
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        bool IExtensionMetadata.IsAvaliableForPermissions => true;
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] {};
        public IEnumerable<Script> Scripts => new Script[] {};
        public IEnumerable<MenuGroup> MenuGroups => new[]
        {
            new MenuGroup(
                "Content",
                1000,
                new[]
                {
                    new MenuItem("/chinook", "Chinook", 100),
                    new MenuItem("/chinook/protected", "Chinook (protected)", 101)
                },
                "fa-files-o"
            ),
            new MenuGroup(
                "Administration",
                2000,
                new[]
                {
                    new MenuItem("/chinook/admin", "Chinook", 100)
                },
                "fa-gears"
            ),
            // A test empty menu group
            new MenuGroup(
                "Other",
                3000,
                null),
        };
    }
}
