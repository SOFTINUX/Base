// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using SoftinuxBase.Infrastructure;
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Common.Attributes;
using SoftinuxBase.Security.Common.Enums;

namespace SoftinuxBase.Security
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        /// <inheritdoc />
        public Assembly CurrentAssembly => Assembly.GetExecutingAssembly();

        /// <inheritdoc />
        public string CurrentAssemblyPath => CurrentAssembly.Location;

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
        bool IExtensionMetadata.IsAvailableForPermissions => true;

        /// <inheritdoc />
        public IEnumerable<StyleSheet> StyleSheets => new[] { new StyleSheet("/Styles.Security.css", 510), };

        /// <inheritdoc />
        public IEnumerable<Script> Scripts => new[]
        {
#if DEBUG
            new Script("/Scripts.security_user.js", 710, Script.JsType.IsModule),
#else
            new Script("/Scripts.security_user.min.js", 710, Script.JsType.IsModule),
#endif
        };

        /// <inheritdoc />
        public IEnumerable<MenuGroup> MenuGroups
        {
            get
            {
                MenuItem[] menuItems = new[]
                {
                    new MenuItem(
                        "/administration",
                        "Main",
                        100,
                        FontAwesomeIcon.IconType.Far,
                        infrastructureAuthorizeAttributes_: new List<PermissionRequirementAttribute>(new[]
                        {
                            new PermissionRequirementAttribute(
                                Permission.Admin,
                                Constants.SoftinuxBaseSecurity),
                        }))
                };
                return new[]
                {
                    new MenuGroup(
                        "Administration",
                        0, // Always first
                        menuItems,
                        FontAwesomeIcon.IconType.Fas,
                        "fa-wrench")
                };
            }
        }
    }
}