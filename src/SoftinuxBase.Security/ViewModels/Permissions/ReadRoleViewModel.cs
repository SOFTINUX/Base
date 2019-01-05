// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    /// <summary>
    /// All the data related to a role, to display for edition of this role.
    /// </summary>
    public class ReadRoleViewModel
    {
        public IdentityRole<string> Role { get; set; }
        public IList<SelectedExtension> SelectedExtensions {get; set;}
        public IList<string> AvailableExtensions {get; set;}
    }
}