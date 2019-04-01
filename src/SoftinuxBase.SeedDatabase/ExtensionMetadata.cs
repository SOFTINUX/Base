// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System ;
using System.Collections.Generic;
using System.Reflection ;
using ExtCore.Infrastructure ;
using SoftinuxBase.Infrastructure;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.SeedDatabase
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public Assembly CurrentAssemnly => Assembly.GetExecutingAssembly();

        /// <summary>
        /// Gets the name of the extension.
        /// </summary>
        public string Name => Attribute.GetCustomAttribute(CurrentAssemnly, typeof(AssemblyName)).ToString();

        /// <summary>
        /// Gets the URL of the extension.
        /// </summary>
        public string Url => Attribute.GetCustomAttribute(CurrentAssemnly, typeof(AssemblyTitleAttribute)).ToString();

        /// <summary>
        /// Gets the version of the extension.
        /// </summary>
        public string Version => Attribute.GetCustomAttribute(CurrentAssemnly, typeof(AssemblyVersionAttribute)).ToString();

        /// <summary>
        /// Gets the authors of the extension (separated by commas).
        /// </summary>
        public string Authors => Attribute.GetCustomAttribute(CurrentAssemnly, typeof(AssemblyCompanyAttribute)).ToString();
        
        /// <summary>
        /// Gets the authors of the extension (separated by commas).
        /// </summary>
        public string Description => Attribute.GetCustomAttribute(CurrentAssemnly, typeof(AssemblyDescriptionAttribute)).ToString();

        bool IExtensionMetadata.IsAvailableForPermissions => false;
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[] { };

        public IEnumerable<Script> Scripts => new Script[] { };

        public IEnumerable<MenuGroup> MenuGroups => new MenuGroup[] { };
    }
}