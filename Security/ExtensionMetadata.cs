using System.Collections.Generic;
using Infrastructure;

namespace Security
{
        public class ExtensionMetadata : IExtensionMetadata
        {
            public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] {
                new StyleSheet("/Styles.Security.css",510),
             };
            public IEnumerable<Script> Scripts => new Script[] { };
            public IEnumerable<MenuGroup> MenuGroups => new MenuGroup[] { };
        }
}
