// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using SoftinuxBase.Security.Permissions.Attributes;

namespace SoftinuxBase.Infrastructure
{
    public class MenuItem
    {
        /// <summary>
        /// If not empty, any of these roles is required (access granted if a single role is possessed by current user).
        /// Menu item is decorated with Microsoft.AspNetCore.Authorization.AuthorizeAttribute with Roles.
        /// </summary>
        private readonly List<string> _anyRequiredRoles = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        /// <param name="url_">set menu item route url.</param>
        /// <param name="name_">set menu item display name.</param>
        /// <param name="position_">set menu item position in menu group.</param>
        /// <param name="fontAwesomeIconType_">type for font awesome icon.</param>
        /// <param name="fontAwesomeIconClass_">set menu item icon. If null, "fa-circle-o" will be used.</param>
        /// <param name="infrastructureAuthorizeAttributes_">set a list of <see cref="PermissionRequirementAttribute"/>.</param>
        /// <param name="microsoftAuthorizeAttributes_">set a list of <see cref="Microsoft.AspNetCore.Authorization.AuthorizeAttribute"/>.</param>
        [Obsolete]
        public MenuItem(
            string url_,
            string name_,
            uint position_,
            FontAwesomeIcon.IconType fontAwesomeIconType_ = FontAwesomeIcon.IconType.far,
            string fontAwesomeIconClass_ = "fa-circle",
            List<PermissionRequirementAttribute> infrastructureAuthorizeAttributes_ = null,
            List<Microsoft.AspNetCore.Authorization.AuthorizeAttribute> microsoftAuthorizeAttributes_ = null)
        {
            Url = url_;
            Name = name_;
            Position = position_;
            FontAwesomeIconType = fontAwesomeIconType_;
            FontAwesomeIconClass = fontAwesomeIconClass_;

            if (microsoftAuthorizeAttributes_ != null)
            {
                // Get the authorized roles and policies
                foreach (var attr in microsoftAuthorizeAttributes_)
                {
                    if (!string.IsNullOrWhiteSpace(attr.Roles))
                    {
                        _anyRequiredRoles.AddRange(attr.Roles.Split(','));
                    }

                    if (!string.IsNullOrWhiteSpace(attr.Policy))
                    {
                        // _allRequiredPermissionIdentifiers.Add(attr.Policy);
                    }
                }
            }

            if (infrastructureAuthorizeAttributes_ != null)
            {
                // Get the authorized permission identifiers
                foreach (var attr in infrastructureAuthorizeAttributes_)
                {
                    if (!string.IsNullOrWhiteSpace(attr.PermissionIdentifier))
                    {
                        // _allRequiredPermissionIdentifiers.Add(attr.PermissionIdentifier);
                    }
                }
            }
        }

        /// <summary>
        /// Gets route url.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Gets menu item display name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets menu item position in list.
        /// </summary>
        public uint Position { get; }

        /// <summary>
        /// Gets get the type for fontawesome
        /// </summary>
        public FontAwesomeIcon.IconType FontAwesomeIconType { get; }

        /// <summary>
        /// Gets the fa-xxx class to render the associated icon.
        /// </summary>
        public string FontAwesomeIconClass { get; }
    }
}