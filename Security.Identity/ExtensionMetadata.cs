using Infrastructure;
using Infrastructure.Interfaces;
using System.Collections.Generic;

namespace Security.Identity
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public IEnumerable<MenuGroup> MenuGroups => new MenuGroup[] {};

        public IEnumerable<Script> Scripts => new Script[] {};

        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] {};

        bool IExtensionMetadata.IsAvailableForPermissions => true;

    }
}