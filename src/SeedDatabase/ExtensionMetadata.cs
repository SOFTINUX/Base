using System.Collections.Generic;
using SoftinuxBase.Infrastructure;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SeedDatabase
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        bool IExtensionMetadata.IsAvailableForPermissions => false;
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet [] {};

        public IEnumerable<Script> Scripts => new Script [] {};

        public IEnumerable<MenuGroup> MenuGroups => new MenuGroup [] {};
    }
}