// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CommonTest;
using Xunit;

namespace CommonTest
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationStorageContext>
    {
        public ApplicationStorageContext CreateDbContext(string[] args)
        {
            return new ApplicationStorageContext(DatabaseFixture.GetDbContextOptionsBuilder().Options);
        }
    }
}