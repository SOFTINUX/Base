// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace SoftinuxBase.WebApp
{
    /// <summary>
    /// Class that holds the Entity Framework DbContext's DbSets related to some extension
    /// (entities in XXX.Data.Entities project) and that also inherits from ExtCore's IStorageContext.
    /// </summary>
    public class ApplicationStorageContext : SoftinuxBase.WebApplication.ApplicationStorageContext
    {
        // No additional DbSet in this webapp

         public ApplicationStorageContext(DbContextOptions options_)
            : base(options_)
        {
        }
    }

}

