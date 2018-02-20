// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

namespace Security.ViewModels.Permissions
{
    /// <summary>
    /// The view model for global permissions granting: for a role, a group or a user, what kind of permissions is granted.
    /// </summary>
    public class GlobalGrantViewModel
    {
        /// <summary>
        /// For every role, the attributed permissions.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Infrastructure.Enums.Permission>> RolePermissions {get; set;}
        /// <summary>
        /// For every group, the attributed permissions.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Infrastructure.Enums.Permission>> GroupPermissions {get; set;}
        /// <summary>
        /// For every user, the attributed permissions.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Infrastructure.Enums.Permission>> UserPermissions {get; set;}

        public GlobalGrantViewModel()
        {
            RolePermissions = new Dictionary<string, List<Infrastructure.Enums.Permission>>();
            GroupPermissions = new Dictionary<string, List<Infrastructure.Enums.Permission>>();
            UserPermissions = new Dictionary<string, List<Infrastructure.Enums.Permission>>();
        }
    }
}