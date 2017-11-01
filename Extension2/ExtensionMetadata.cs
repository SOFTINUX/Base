using System.Collections.Generic;
using Infrastructure;

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
