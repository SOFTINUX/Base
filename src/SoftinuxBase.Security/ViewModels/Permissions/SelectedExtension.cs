// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    /// <summary>
    /// An extension name and its <see cref="SelectablePermissionDisplay"/>, grouped by permission section, each permission display having a selection indicator to describe a link to an entity like a role or a user.
    /// </summary>
    public class SelectedExtension
    {
        public SelectedExtension(string extensionName_)
        {
            ExtensionName = extensionName_;
            GroupedBySectionPermissionDisplays = new Dictionary<string, List<SelectablePermissionDisplay>>();
        }

        public string ExtensionName { get; }

        public Dictionary<string, List<SelectablePermissionDisplay>> GroupedBySectionPermissionDisplays { get; }
    }
}