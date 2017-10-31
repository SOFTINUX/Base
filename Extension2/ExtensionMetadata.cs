using System.Collections.Generic;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;

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
        
        public IEnumerable<IAuthorizationPolicyProvider> AuthorizationPolicyProviders => null;
    }
}
