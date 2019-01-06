// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace WebApplication
{
    /// <summary>
    /// Class that holds the Entity Framework DbContext's DbSets related to some extension
    /// (entities in XXX.Data.Entities project) and that also inherits from ExtCore's IStorageContext.
    /// </summary>
    public class ApplicationStorageContext : SoftinuxBase.WebApplication.ApplicationStorageContext
    {
        // No additional DbSet in this webapp

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStorageContext"/> class.
        ///
        /// </summary>
        /// <param name="options_">DB Context options.</param>
        public ApplicationStorageContext(DbContextOptions options_)
            : base(options_)
        {
        }
    }
}