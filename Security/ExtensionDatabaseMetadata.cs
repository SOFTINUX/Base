// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Infrastructure;

namespace Security
{
        public class ExtensionDatabaseMetadata : IExtensionDatabaseMetadata
        {
            public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] {
                new StyleSheet("/Styles.Security.css",510),
             };
            public IEnumerable<Script> Scripts => new Script[] { };
            public IEnumerable<MenuGroup> MenuGroups => new MenuGroup[] { };

        }
}
