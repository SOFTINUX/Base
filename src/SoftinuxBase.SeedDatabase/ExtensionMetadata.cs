// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using SoftinuxBase.Infrastructure;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.SeedDatabase
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        /// <inheritdoc />
        public Assembly CurrentAssembly => Assembly.GetExecutingAssembly();

        /// <inheritdoc />
        public string CurrentAssemblyPath => CurrentAssembly.Location;

        /// <inheritdoc />
        public Type Permissions => null;

        /// <inheritdoc />
        public string FileExtensionPrefix { get { return string.Empty; } }

        /// <inheritdoc />
        public string Name => CurrentAssembly.GetName().Name;

        /// <inheritdoc />
        public string Url => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyTitleAttribute))?.ToString();

        /// <inheritdoc />
        public string Version => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyVersionAttribute))?.ToString();

        /// <inheritdoc />
        public string Authors => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyCompanyAttribute))?.ToString();

        /// <inheritdoc />
        public string Description => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyDescriptionAttribute))?.ToString();

        /// <inheritdoc />
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] { };

        /// <inheritdoc />
        public IEnumerable<Script> Scripts => new Script[] { };

        /// <inheritdoc />
        public IEnumerable<MenuGroup> MenuGroups => new MenuGroup[] { };
    }
}