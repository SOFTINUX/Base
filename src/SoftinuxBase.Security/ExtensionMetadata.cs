// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using SoftinuxBase.Common;
using SoftinuxBase.Infrastructure;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Common.Attributes;
using SoftinuxBase.Security.Common.Enums;

namespace SoftinuxBase.Security
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        /// <summary>
        /// Gets the current assembly object.
        /// </summary>
        public Assembly CurrentAssembly => Assembly.GetExecutingAssembly();

        /// <summary>
        /// Gets the full path with assembly name.
        /// </summary>
        public string CurrentAssemblyPath => CurrentAssembly.Location;

        /// <summary>
        /// Gets the name of the extension.
        /// </summary>
        public string Name => CurrentAssembly.GetName().Name;

        /// <summary>
        /// Gets the URL of the extension.
        /// </summary>
        public string Url => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyTitleAttribute)).ToString();

        /// <summary>
        /// Gets the version of the extension.
        /// </summary>
        public string Version => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyVersionAttribute)).ToString();

        /// <summary>
        /// Gets the authors of the extension (separated by commas).
        /// </summary>
        public string Authors => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyCompanyAttribute)).ToString();

        /// <summary>
        /// Gets the description of the extension (separated by commas).
        /// </summary>
        public string Description => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyDescriptionAttribute)).ToString();

        bool IExtensionMetadata.IsAvailableForPermissions => true;

        public IEnumerable<StyleSheet> StyleSheets => new[]
        {
                new StyleSheet("/Styles.Security.css", 510),
        };
        public IEnumerable<Script> Scripts => new Script[]
        {
#if DEBUG
            new Script("/Scripts.security_user.js", 710),
#else
            new Script("/Scripts.security_user.min.js", 710),
#endif
        };

        public IEnumerable<MenuGroup> MenuGroups
        {
            get
            {
                MenuItem[] menuItems_ = new[]
                                    {
                        new MenuItem("/administration", "Main", 100, null, new List<PermissionRequirementAttribute>(new[] { new PermissionRequirementAttribute(Permission.Admin, Constants.SoftinuxBaseSecurity), }))
                                    };
                return new MenuGroup[]
                {
                    new MenuGroup(
                        "Administration",
                        0, // Always first
                        menuItems_,
                        "fa-wrench")
                };
            }
        }
    }
}
