// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    /// <summary>
    /// Implementing this interface allows your extension to define one or more menu items,
    /// and provide script and stylesheets elements to the main web application.
    /// </summary>
    public interface IExtensionMetadata
    {
        IEnumerable<StyleSheet> StyleSheets { get; }
        IEnumerable<Script> Scripts { get; }
        IEnumerable<MenuGroup> MenuGroups { get; }
        bool IsAvailableForPermissions { get; }
    }
}
