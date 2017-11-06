using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public interface IExtensionMetadata
    {
        IEnumerable<StyleSheet> StyleSheets { get; }
        IEnumerable<Script> Scripts { get; }
        IEnumerable<MenuGroup> MenuGroups { get; }
    }
}
