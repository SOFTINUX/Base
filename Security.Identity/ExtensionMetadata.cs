using Infrastructure;
using Infrastructure.Interfaces;
using System.Collections.Generic;

namespace Security.Identity
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public IEnumerable<MenuGroup> MenuGroups => null;

        public IEnumerable<Script> Scripts => null;

        public IEnumerable<StyleSheet> StyleSheets => null;

        bool IExtensionMetadata.IsAvailableForPermissions => true;

    }
}