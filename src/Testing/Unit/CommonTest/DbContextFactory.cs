// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using Microsoft.EntityFrameworkCore.Design;

namespace CommonTest
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationStorageContext>
    {
        public ApplicationStorageContext CreateDbContext(string[] args_)
        {
            return new ApplicationStorageContext(DatabaseFixture.GetDbContextOptionsBuilder().Options);
        }
    }
}