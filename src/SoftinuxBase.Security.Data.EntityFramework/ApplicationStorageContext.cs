// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.RefreshClaimsParts;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    /// <summary>
    /// Class that holds the Entity Framework DbContext's DbSets related to some extension
    /// (entities in XXX.Data.Entities project) and that also inherits from ExtCore's IStorageContext.
    /// It's partial so that you can add your own DbSets.
    /// </summary>
    public partial class ApplicationStorageContext : IdentityDbContext<User, IdentityRole<string>, string>, IStorageContext, ITimeStore
    {
        public ApplicationStorageContext(DbContextOptions options_) : base(options_) { }

        protected ApplicationStorageContext(DbContextOptions options_, IAuthChanges authChange_)
            : base(options_)
        {
            _authChange = authChange_;
        }

        // Old permissions system - to be removed
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }

        // New permissions system
        private readonly IAuthChanges _authChange;

        public DbSet<UserToRole> UserToRoles { get; set; }
        public DbSet<RoleToPermissions> RolesToPermissions { get; set; }
        public DbSet<ModulesForUser> ModulesForUsers { get; set; }

        /// <summary>
        /// Gets or sets the TimeStore DbSet.
        /// The TimeStore holds the time when a change is made to the Roles/Permission such that it might alter a user's permissions.
        /// This data will be used if you want the "refresh user claims" feature.
        /// </summary>
        public DbSet<TimeStore> TimeStores { get; set; }

        /// <summary>
        /// Override used only if you want the "refresh user claims" feature (active by configuration).
        /// I only have to override these two versions of SaveChanges, as the other two SaveChanges versions call these.
        /// <param name="acceptAllChangesOnSuccess_"></param>
        /// <returns></returns>
        /// </summary>
        public override int SaveChanges(bool acceptAllChangesOnSuccess_)
        {
            if (_authChange == null)
            {
                // _authChange is null if not using UpdateCookieOnChange, so bypass permission change code
                return base.SaveChanges(acceptAllChangesOnSuccess_);
            }

            if (this.UserPermissionsMayHaveChanged())
            {
                _authChange.AddOrUpdate(this);
            }

            return base.SaveChanges(acceptAllChangesOnSuccess_);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess_, CancellationToken cancellationToken_ = default(CancellationToken))
        {
            if (_authChange == null)
            {
                // _authChange is null if not using UpdateCookieOnChange, so bypass permission change code
                return await base.SaveChangesAsync(acceptAllChangesOnSuccess_, cancellationToken_);
            }

            if (this.UserPermissionsMayHaveChanged())
            {
                _authChange?.AddOrUpdate(this);
            }

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess_, cancellationToken_);
        }

        //-------------------------------------------------------
        // The ITimeStore methods
        public long? GetValueFromStore(string key_)
        {
            return Find<TimeStore>(key_)?.LastUpdatedTicks;
        }

        public void AddUpdateValue(string key_, long cachedTicks_)
        {
            var currentEntry = Find<TimeStore>(key_);
            if (currentEntry != null)
            {
                currentEntry.LastUpdatedTicks = cachedTicks_;
            }
            else
            {
                Add(new TimeStore { Key = key_, LastUpdatedTicks = cachedTicks_ });
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder_)
        {
#if DEBUG
            base.OnConfiguring(optionsBuilder_.EnableSensitiveDataLogging().UseLoggerFactory(GetLoggerFactory()));
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder_)
        {
            base.OnModelCreating(modelBuilder_);
            this.RegisterEntities(modelBuilder_);

            // New permissions system
            modelBuilder_.TenantBaseConfig();
            modelBuilder_.ExtraAuthorizeConfig();
        }



        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder_ =>
                builder_.AddProvider(new EfLoggerProvider())
                    .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug));
            return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }
    }
}