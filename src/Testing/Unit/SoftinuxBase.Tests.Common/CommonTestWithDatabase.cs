// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SoftinuxBase.Tests.Common
{
    /// <summary>
    /// Base class for all test classes that should use a database.
    /// This class ensures that permissions exist in database.
    ///
    /// For base roles to exist, you must call "await CreateBaseRolesIfNeeded()".
    ///
    /// A derived class should be decorated with "[Collection("Database collection")]", using the DatabaseCollection class defined in its assembly.
    /// </summary>
    public class CommonTestWithDatabase : IDisposable
    {
        protected DatabaseFixture DatabaseFixture { get; set; }

        public CommonTestWithDatabase(DatabaseFixture databaseFixture_)
        {
            DatabaseFixture = databaseFixture_;
        }

        public void Dispose()
        {
            CleanTrackedEntities();
        }

        protected void CleanTrackedEntities()
        {
            var changedEntriesCopy = ((DbContext)DatabaseFixture.Storage.StorageContext).ChangeTracker.Entries()

                // .Where(e => e.State == EntityState.Added ||
                //            e.State == EntityState.Modified ||
                //            e.State == EntityState.Deleted)
                .ToList();
            foreach (var entity in changedEntriesCopy)
            {
                ((DbContext)DatabaseFixture.Storage.StorageContext).Entry(entity.Entity).State = EntityState.Detached;
            }
        }
    }
}