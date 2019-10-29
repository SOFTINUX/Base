// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.DataKeyParts;
using SoftinuxBase.Security.DataLayer.EfCode.Configurations;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses;
using SoftinuxBase.Security.DataLayer.MultiTenantClasses;

namespace SoftinuxBase.Security.DataLayer.EfCode
{
    public class CompanyDbContext : DbContext
    {
        internal readonly string DataKey;

        public DbSet<TenantBase> Tenants { get; set; }
        public DbSet<ShopStock> ShopStocks { get; set; }
        public DbSet<ShopSale> ShopSales { get; set; }

        public DbSet<TimeStore> TimeStores { get; set; }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options, IGetClaimsProvider claimsProvider)
            : base(options)
        {
            DataKey = claimsProvider.DataKey;
        }

        //I only have to override these two version of SaveChanges, as the other two SaveChanges versions call these
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.MarkWithDataKeyIfNeeded(DataKey);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.MarkWithDataKeyIfNeeded(DataKey);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.TenantBaseConfig();
            modelBuilder.CompanyDbConfig(this);
        }
    }
}