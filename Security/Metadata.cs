using System.Collections.Generic;
using Infrastructure;

namespace Security
{
    public class Metadata
    {
        public class Metadata : IExtensionMetadata
        {
            public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] { };
            public IEnumerable<Script> Scripts => new Script[] { };
            public IEnumerable<MenuGroup> MenuGroups => new MenuGroup[] { };
        }
    }
}
