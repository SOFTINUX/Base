// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between users and roles.
    /// </summary>
    public class UserRole : IdentityUserRole<int>, IEntity
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
