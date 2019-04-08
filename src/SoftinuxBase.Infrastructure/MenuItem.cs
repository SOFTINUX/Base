// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using SoftinuxBase.Security.Common.Attributes;

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
        /// If not empty, all these values are required (access granted if all matching Permission claims are possessed by current user).
        /// Menu item is decorated with either Microsoft.AspNetCore.Authorization.AuthorizeAttribute with Policy
        /// or Infrastructure.Attributes.PermissionRequirementAttribute.
        /// </summary>
        private readonly List<string> _allRequiredPermissionIdentifiers = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        /// <param name="url_">set menuitem route url.</param>
        /// <param name="name_">set menu item display name.</param>
        /// <param name="position_">set menu item position in menu group.</param>
        /// <param name="fontAwesomeClass_">set menu item icon.</param>
        /// <param name="infrastructureAuthorizeAttributes_"></param>
        /// <param name="microsoftAuthorizeAttributes_"></param>
        public MenuItem(
            string url_,
            string name_,
            uint position_,
            string fontAwesomeClass_ = null,
            List<PermissionRequirementAttribute> infrastructureAuthorizeAttributes_ = null,
            List<Microsoft.AspNetCore.Authorization.AuthorizeAttribute> microsoftAuthorizeAttributes_ = null)
        {
            Url = url_;
            Name = name_;
            Position = position_;

            // Always a default icon
            FontAwesomeClass = fontAwesomeClass_ ?? "fa-circle-o";

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
                        _allRequiredPermissionIdentifiers.Add(attr.Policy);
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
                        _allRequiredPermissionIdentifiers.Add(attr.PermissionIdentifier);
                    }
                }
            }
        }

        /// <summary>
        /// Gets route url.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Gets menu display name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets menu position in list.
        /// </summary>
        public uint Position { get; }

        /// <summary>
        /// Gets the fa-xxx class to render the associated icon.
        /// </summary>
        public string FontAwesomeClass { get; }
    }
}