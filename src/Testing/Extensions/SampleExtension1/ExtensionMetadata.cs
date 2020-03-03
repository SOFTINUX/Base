// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using SoftinuxBase.Infrastructure;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SampleExtension1
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public string Name { get { return "SampleExtension1"; } }
        public string Description { get { return "Sample Extension 1"; } }
        public string Url { get { return null; } }
        public string Version { get { return "1.0"; } }
        public string Authors { get { return "Softinux"; } }
        public IEnumerable<StyleSheet> StyleSheets { get { return null; } }
        public IEnumerable<Script> Scripts { get { return null; } }
        public IEnumerable<MenuGroup> MenuGroups { get { return null; } }
        public bool IsAvailableForPermissions { get { return true; } }

        /// <inheritdoc />
        public Assembly CurrentAssembly => Assembly.GetExecutingAssembly();

        /// <inheritdoc />
        public string CurrentAssemblyPath => CurrentAssembly.Location;

        public Type Permissions { get { return typeof(SamplePermissions); } }
    }
}