using System.Collections.Generic;
using Infrastructure;

namespace Extension1
{
    public class Metadata : IExtensionMetadata
    {
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] {};
        public IEnumerable<Script> Scripts => new Script[] {};
        public IEnumerable<MenuItem> MenuItems
        {
            get
            {
                return new MenuItem[]
                {
                    new MenuItem("/extension1", "Exstension 1", 100)
                };
            }
        }
    }
}
