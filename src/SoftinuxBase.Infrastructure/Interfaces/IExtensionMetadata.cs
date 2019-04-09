// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Reflection;
using ExtCore.Infrastructure;

namespace SoftinuxBase.Infrastructure.Interfaces
{
    /// <summary>
    /// Implement this interface to allow your extension to define one or more menu items,
    /// and provide scripts and stylesheets elements to include in the main web application.
    /// </summary>
    public interface IExtensionMetadata : IExtension
    {
        /// <summary>
        /// Gets list of style sheets of the extension.
        /// </summary>
        IEnumerable<StyleSheet> StyleSheets { get; }

        /// <summary>
        /// Gets list of scripts of the extension.
        /// </summary>
        IEnumerable<Script> Scripts { get; }

        /// <summary>
        /// Gets menu group of the extension.
        /// </summary>
        IEnumerable<MenuGroup> MenuGroups { get; }

        /// <summary>
        /// Gets a value indicating whether the extension is visible in permissions configuration panel.
        /// </summary>
        bool IsAvailableForPermissions { get; }

        /// <summary>
        /// Gets extension assembly.
        /// </summary>
        Assembly CurrentAssembly { get; }

        /// <summary>
        /// Gets path to extension assembly location.
        /// </summary>
        string CurrentAssemblyPath { get; }
    }
}
