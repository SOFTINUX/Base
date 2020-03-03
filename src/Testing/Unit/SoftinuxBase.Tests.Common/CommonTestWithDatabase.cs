// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using Permission = SoftinuxBase.Security.Permissions.Enums.Permission;

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

            if (repo.All().FirstOrDefault(p_ => p_.Name == Permission.Admin.GetPermissionName()) != null)
            {
                return;
            }

            // Base permissions not already created
            Permission[] permissions = (Permission[])Enum.GetValues(typeof(Permission));

            foreach (var p in permissions)
            {
                // create a permission object out of the enum value
                SoftinuxBase.Security.Data.Entities.Permission permission = new SoftinuxBase.Security.Data.Entities.Permission()
                {
                    // Automatic ID
                    Name = PermissionHelper.GetPermissionName(p)
                };

                repo.Create(permission);
            }

            // Can't use the async Storage.SaveAsync() because this method is called in class constructor - TODO use a helper pattern?
            DatabaseFixture.Storage.Save();
        }

        /// <summary>
        /// Create a role, if it doesn't exist
        /// Similar to SoftinuxBase.SeedDatabase extension's job.
        /// </summary>
        /// <param name="roleName_">Role name.</param>
        /// <returns>The asynchronous Task.</returns>
        protected async Task CreateRoleIfNotExistingAsync(string roleName_)
        {
            // create an identity role object
            IdentityRole<string> identityRole = new IdentityRole<string>
            {
                // Automatic ID
                Name = roleName_
            };

            if (!await DatabaseFixture.RoleManager.RoleExistsAsync(identityRole.Name))
            {
                var result = await DatabaseFixture.RoleManager.CreateAsync(identityRole);

                if (!result.Succeeded)
                {
                    throw new Exception($"Could not create missing role: {identityRole.Name}");
                }
            }
        }

        /// <summary>
        /// Create a user, if it doesn't exist
        /// Similar to SoftinuxBase.SeedDatabase extension's job.
        /// </summary>
        /// <param name="userName_">User name/first name/last name/part of e-mail.</param>
        /// <returns>The asynchronous Task.</returns>
        protected async Task<User> CreateUserAsync(string userName_)
        {
            // create an identity user object
            User user = new User { FirstName = userName_, LastName = userName_, UserName = userName_, Email = $"{userName_}@softinux.com", LockoutEnabled = false };
            var result = await DatabaseFixture.UserManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception($"Could not create user with name: {userName_}");
            }

            return user;
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