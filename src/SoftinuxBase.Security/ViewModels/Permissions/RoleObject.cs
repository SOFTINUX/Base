// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.Data.Entities;
using Permission = SoftinuxBase.Security.Common.Enums.Permission;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    public class RoleObject
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="role_">Required parameter.</param>
        /// <param name="users_">Optional parameter.</param>
        /// <param name="extensions_">Extensions list, optional parameter.</param>
        /// <param name="permissions_">Permissions list optional parameter.</param>
        public RoleObject(IdentityRole<string> role_, List<User> users_ = null, List<string> extensions_ = null, List<Permission> permissions_ = null)
        {
            RoleNormalizedName = role_.NormalizedName;
            ConcurrencyStamp = role_.ConcurrencyStamp;
            RoleName = role_.Name;
            RoleId = role_.Id;
            Extensions = extensions_;
            Users = users_;
            Permissions = permissions_;
        }

        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleNormalizedName { get; }
        public string ConcurrencyStamp { get; }
        public List<Permission> Permissions { get; private set; }
        public List<string> Extensions { get; private set; }
        public List<User> Users { get; }
    }
}
