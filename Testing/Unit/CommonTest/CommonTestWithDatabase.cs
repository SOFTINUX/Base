// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Security.Common;
using Security.Common.Enums;
using Security.Data.Abstractions;
using Xunit;

namespace CommonTest
{
    /// <summary>
    /// Base class for all test classes that should use a database.
    /// A derived class should be decorated with "[Collection("Database collection")]", using the DatabaseCollection class defined in its assembly.
    /// </summary>
    public class CommonTestWithDatabase
    {
        public CommonTestWithDatabase(DatabaseFixture databaseFixture_)
        {
            this.DatabaseFixture = databaseFixture_;
            CreatePermissionsIfNeeded();
        }
        protected DatabaseFixture DatabaseFixture { get; set; }

        /// <summary>
        /// Create the standard records in Permission table, if they don't exist.
        /// </summary>
        private void CreatePermissionsIfNeeded()
        {
            var repo = DatabaseFixture.Storage.GetRepository<IPermissionRepository>();

            if (repo.All().FirstOrDefault(p_ => p_.Id == Permission.Admin.ToString()) != null)
                return;

            // Base permissions not already created

            Permission[] permissions = (Permission[])Enum.GetValues(typeof(Permission));

            foreach (var p in permissions)
            {
                // create a permission object out of the enum value
                Security.Data.Entities.Permission permission = new Security.Data.Entities.Permission()
                {
                    Id = PermissionHelper.GetPermissionName(p),
                    Name = PermissionHelper.GetPermissionName(p)
                };

                repo.Create(permission);
            }

            DatabaseFixture.Storage.Save();

        }

        protected void CleanTrackedEntities()
        {
            var changedEntriesCopy = ((DbContext)DatabaseFixture.Storage.StorageContext).ChangeTracker.Entries()	
                //.Where(e => e.State == EntityState.Added ||	
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