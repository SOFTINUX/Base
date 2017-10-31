using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure
{
    public interface IExtensionMetadata
    {
        IEnumerable<StyleSheet> StyleSheets {get;}
        IEnumerable<Script> Scripts {get;}
        IEnumerable<MenuGroup> MenuGroups {get;}
        IEnumerable<IAuthorizationPolicyProvider> AuthorizationPolicyProviders { get; }
    }
}
