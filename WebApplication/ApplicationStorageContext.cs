using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Security.Data.Entities;
using Security.Data.EntityFramework.Util;

namespace WebApplication
{
    public partial class ApplicationStorageContext : IdentityDbContext<User, Role, string>, IStorageContext
    {
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupPermission> GroupPermission { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }

        public ApplicationStorageContext(DbContextOptions<ApplicationStorageContext> options_)
            : base(options_)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder_)
        {
#if DEBUG
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EfLoggerProvider());
            base.OnConfiguring(optionsBuilder_.EnableSensitiveDataLogging().UseLoggerFactory(loggerFactory));
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder_)
        {
            base.OnModelCreating(modelBuilder_);
            this.RegisterEntities(modelBuilder_);
        }

    }
}

