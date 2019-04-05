// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Reflection;
using ExtCore.Infrastructure;

namespace SoftinuxBase.Infrastructure.Interfaces
{
    /// <summary>
    /// Implementing this interface allows your extension to define one or more menu items,
    /// and provide script and stylesheets elements to the main web application.
    /// </summary>
    public interface IExtensionMetadata : IExtension
    {
        IEnumerable<StyleSheet> StyleSheets { get; }
        IEnumerable<Script> Scripts { get; }
        IEnumerable<MenuGroup> MenuGroups { get; }
        bool IsAvailableForPermissions { get; }

        Assembly CurrentAssembly { get; }

#if DEBUG
        string CurrentAssemblyPath { get; }
#endif
    }
}
