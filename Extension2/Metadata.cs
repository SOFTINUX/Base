using System.Collections.Generic;
using Infrastructure;

namespace Extension2
{
    public class Metadata : IExtensionMetadata
    {
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] {};
        public IEnumerable<Script> Scripts => new Script[] {};
        public IEnumerable<MenuGroup> MenuGroups
        {
            get
            {
                return new MenuGroup[]
                {
                    new MenuGroup(
                        "Content",
                        1000,
                        new MenuItem[]
                        {
                            new MenuItem("/extension2", "Extension 2", 101)
                        }
                    )
                };
            }
        }
    }
}
