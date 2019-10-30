// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.DataLayer.EfCode.Configurations;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses.Support;
using SoftinuxBase.Security.RefreshClaimsParts;

namespace SoftinuxBase.Security.DataLayer.EfCode
{
    public class ExtraAuthorizeDbContext : DbContext, ITimeStore
    {
        private readonly IAuthChanges _authChange;

        public DbSet<UserToRole> UserToRoles { get; set; }
        public DbSet<RoleToPermissions> RolesToPermissions { get; set; }
        public DbSet<ModulesForUser> ModulesForUsers { get; set; }

        /// <summary>
        /// The TimeStore holds the time when a change is made to the Roles/Permission such that it might alter a user's permissions.
        /// //YOU ONLY NEED THIS IF YOU WANT THE "REFRESH USER CLAIMS" FEATURE
        /// </summary>
        public DbSet<TimeStore> TimeStores { get; set; }

        //YOU ONLY NEED TO OVERRIDE SAVECHANGES IF YOU WANT THE "REFRESH USER CLAIMS" FEATURE
        //I only have to override these two versions of SaveChanges, as the other two SaveChanges versions call these
        public override int SaveChanges(bool acceptAllChangesOnSuccess_)
        {
            if (_authChange == null)
                //_authChange is null if not using UpdateCookieOnChange, so bypass permission change code
                return base.SaveChanges(acceptAllChangesOnSuccess_);

            if (this.UserPermissionsMayHaveChanged())
                _authChange.AddOrUpdate(this);
            return base.SaveChanges(acceptAllChangesOnSuccess_);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess_, CancellationToken cancellationToken_ = new CancellationToken())
        {
            if (_authChange == null)
                //_authChange is null if not using UpdateCookieOnChange, so bypass permission change code
                return await base.SaveChangesAsync(acceptAllChangesOnSuccess_, cancellationToken_);

            if (this.UserPermissionsMayHaveChanged())
                _authChange?.AddOrUpdate(this);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess_, cancellationToken_);
        }

        public ExtraAuthorizeDbContext(DbContextOptions<ExtraAuthorizeDbContext> options_, IAuthChanges authChange_)
            : base(options_)
        {
            _authChange = authChange_;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder_)
        {
            modelBuilder_.TenantBaseConfig();
            modelBuilder_.ExtraAuthorizeConfig();
        }

        //-------------------------------------------------------
        //The ITimeStore methods


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
                Add(new TimeStore {Key = key_, LastUpdatedTicks = cachedTicks_ });
            }
        }
    }
}