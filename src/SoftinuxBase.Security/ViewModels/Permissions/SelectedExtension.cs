// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    /// <summary>
    /// An extension name and the associated permission level. For example for a link to a role.
    /// </summary>
    public class SelectedExtension
    {
        public string Name { get; set; }
        // permission name
        public string Permission { get; set; }
    }
}