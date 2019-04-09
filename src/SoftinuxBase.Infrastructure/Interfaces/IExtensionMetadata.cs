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
        /// <summary>
        /// Gets list of styles sheet for extension.
        /// </summary>
        IEnumerable<StyleSheet> StyleSheets { get; }

        /// <summary>
        /// Gets list of scripts for extension.
        /// </summary>
        IEnumerable<Script> Scripts { get; }

        /// <summary>
        /// Gets menu group of extensions.
        /// </summary>
        IEnumerable<MenuGroup> MenuGroups { get; }

        /// <summary>
        /// Gets a value indicating whether if extension if visible in permissions panel.
        /// </summary>
        bool IsAvailableForPermissions { get; }

        /// <summary>
        /// Gets extention name.
        /// </summary>
        Assembly CurrentAssembly { get; }

        /// <summary>
        /// Gets path to extension location.
        /// </summary>
        string CurrentAssemblyPath { get; }
    }
}
