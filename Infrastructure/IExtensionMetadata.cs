using System.Collections.Generic;

namespace Infrastructure
{
    public interface IExtensionMetadata
    {
        IEnumerable<StyleSheet> StyleSheets {get;}
        IEnumerable<Script> Scripts {get;}
        IEnumerable<MenuItem> MenuItems {get;}  //TODO make it groupe menu items
    }
}
