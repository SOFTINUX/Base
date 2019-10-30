// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.DataLayer.EfCode.Configurations;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses;

namespace SoftinuxBase.Security.DataLayer.EfCode
{
    /// <summary>
    /// This only exists to create one database that covers both ExtraAuthorizeDbContext and CompanyDbContext
    /// </summary>
    public class CombinedDbContext : DbContext
    {
        //ExtraAuthorizeDbContext
        public DbSet<UserToRole> UserToRoles { get; set; }
        public DbSet<RoleToPermissions> RolesToPermissions { get; set; }
        public DbSet<ModulesForUser> ModulesForUsers { get; set; }

        //TimeStore data
        public DbSet<TimeStore> TimeStores { get; set; }

        public CombinedDbContext(DbContextOptions<CombinedDbContext> options_)
            : base(options_) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder_)
        {
            modelBuilder_.TenantBaseConfig();
            modelBuilder_.ExtraAuthorizeConfig();
        }
    }
}