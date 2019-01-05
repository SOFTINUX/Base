// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using SoftinuxBase.Security.Common.Attributes;

namespace SoftinuxBase.Infrastructure
{
    public class MenuItem
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public uint Position { get; }

        /// <summary>
        /// The fa-xxx class to render the associated icon
        /// </summary>
        public string FontAwesomeClass { get; }

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

        public MenuItem(string url_, string name_, uint position_, string fontAwesomeClass_ = null,
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
                        _anyRequiredRoles.AddRange(attr.Roles.Split(','));
                    if (!string.IsNullOrWhiteSpace(attr.Policy))
                        _allRequiredPermissionIdentifiers.Add(attr.Policy);
                }
            }

            if (infrastructureAuthorizeAttributes_ != null)
            {
                // Get the authorized permission identifiers
                foreach (var attr in infrastructureAuthorizeAttributes_)
                {
                    if (!string.IsNullOrWhiteSpace(attr.PermissionIdentifier))
                        _allRequiredPermissionIdentifiers.Add(attr.PermissionIdentifier);
                }
            }
        }
    }
}