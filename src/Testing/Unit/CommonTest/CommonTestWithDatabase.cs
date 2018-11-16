// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.SeedDatabase;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Common.Enums;
using SoftinuxBase.Security.Data.Abstractions;

namespace CommonTest
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
            CreatePermissionsIfNeeded();
        }

        public void Dispose()
        {
            CleanTrackedEntities();
        }

        /// <summary>
        /// Create the standard records in Permission table, if they don't exist.
        /// Similar to SoftinuxBase.SeedDatabase extension's job.
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
                SoftinuxBase.Security.Data.Entities.Permission permission = new SoftinuxBase.Security.Data.Entities.Permission()
                {
                    Id = PermissionHelper.GetPermissionName(p),
                    Name = PermissionHelper.GetPermissionName(p)
                };

                repo.Create(permission);
            }

            DatabaseFixture.Storage.Save();
        }

        /// <summary>
        /// Create the standard base roles, if they don't exist.
        /// Similar to SoftinuxBase.SeedDatabase extension's job.
        /// </summary>
        /// <returns></returns>
        protected async Task CreateBaseRolesIfNeeded()
        {
            // Get the list of the role from the enum
            Role[] roles = (Role[])Enum.GetValues(typeof(Role));

            foreach(var r in roles)
            {
                await CreateRoleIfNotExisting(r.GetRoleName());
            }
        }

        /// <summary>
        /// Create a roles, if it doesn't exist
        /// Similar to SoftinuxBase.SeedDatabase extension's job.
        /// </summary>
        /// <param name="roleName_"></param>
        /// <returns></returns>
        protected async Task CreateRoleIfNotExisting(string roleName_)
        {
            // create an identity role object out of the enum value
            IdentityRole<string> identityRole = new IdentityRole<string>
            {
                // Automatic ID
                Name = roleName_
            };

            if(!await DatabaseFixture.RoleManager.RoleExistsAsync(identityRole.Name))
            {
                var result = await DatabaseFixture.RoleManager.CreateAsync(identityRole);

                if(!result.Succeeded)
                {
                    throw new Exception($"Could not create missing role: {identityRole.Name}");
                }
            }
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