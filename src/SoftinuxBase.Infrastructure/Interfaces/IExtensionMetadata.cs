// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using ExtCore.Infrastructure;

namespace SoftinuxBase.Infrastructure.Interfaces
{
    /// <summary>
    /// Implement this interface to allow your extension to:.
    /// <ul><li>define one or more menu items</li>
    /// <li>provide scripts and stylesheets elements to include in the main web application</li>
    /// <li>expose the enumeration that represent permissions provided and used by this extension</li>
    /// </ul>
    /// </summary>
    public interface IExtensionMetadata : IExtension
    {
        /// <summary>
        /// Gets list of style sheets of the extension. Can be null.
        /// </summary>
        IEnumerable<StyleSheet> StyleSheets { get; }

        /// <summary>
        /// Gets list of scripts of the extension. Can be null.
        /// </summary>
        IEnumerable<Script> Scripts { get; }

        /// <summary>
        /// Gets menu group of the extension. Can be null.
        /// </summary>
        IEnumerable<MenuGroup> MenuGroups { get; }

        /// <summary>
        /// Gets extension's assembly.
        /// </summary>
        Assembly CurrentAssembly { get; }

        /// <summary>
        /// Gets path to extension's assembly location.
        /// </summary>
        string CurrentAssemblyPath { get; }

        /// <summary>
        /// Gets the type of the enumeration that defines permissions provided by this extension. Can be null.
        /// When not null, the extension is visible in permissions configuration panel.
        /// </summary>
        Type Permissions { get; }

        /// <summary>
        /// Gets a file extension prefix.
        /// Useful to manage a variable embedded resource path that would use .js or .css file extension when unminified
        /// and .min.js or .min.css when minified.
        /// </summary>
        /// <remarks>
        /// Typical usage: use minified resources when extension is compiled in RELEASE mode.
        /// Sample implementation in your ExtensionMetadata:
        ///
        ///public string FileExtensionPrefix
        ///{
        ///    get
        ///    {
        ///#if DEBUG
        ///        return string.Empty;
        ///#else
        ///        return ".min";
        ///#endif
        ///    }
        ///}
        ///
        /// public IEnumerable&lt;Script&gt; Scripts => new[] {
        ///     new Script($"/Scripts.myScript{FileExtensionPrefix}.js", 710, Script.JsType.IsModule),
        /// };
        /// </remarks>
        string FileExtensionPrefix { get; }
    }
}