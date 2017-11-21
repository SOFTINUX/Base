// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Extension2
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] {};
        public IEnumerable<Script> Scripts => new Script[] {};
        public IEnumerable<MenuGroup> MenuGroups => new []
        {
            new MenuGroup(
                "Content",
                1000,
                new []
                {
                    new MenuItem("/extension2", "Extension 2", 101)
                }
            )
        };
    }
}
