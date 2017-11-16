// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Infrastructure;

namespace Extension1
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] {};
        public IEnumerable<Script> Scripts => new Script[] {};
        public IEnumerable<MenuGroup> MenuGroups => new MenuGroup[]
        {
            new MenuGroup(
                "Content",
                1000,
                new MenuItem[]
                {
                    new MenuItem("/extension1", "Extension 1", 100)
                }
            ),
            new MenuGroup(
                "Administration",
                2000,
                new MenuItem[]
                {
                    new MenuItem("/extension1/admin", "Extension 1", 100)
                }
            )
        };
    }
}
