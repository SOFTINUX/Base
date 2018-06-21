using System;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Infrastructure.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Security.Data.Entities;
using CommonTest;

namespace CommonTest
{
    public partial class ApplicationStorageContext : IdentityDbContext<User, IdentityRole<string>, string>, IStorageContext
    {
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }

        public ApplicationStorageContext(DbContextOptions<ApplicationStorageContext> options_)
            : base(options_)
        {
            Console.WriteLine("ApplicationStorageContext: Constructor");
            Utilities.LoadExtensions();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder_)
        {
            Console.WriteLine("ApplicationStorageContext: OnConfiguring");
#if DEBUG
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EfLoggerProvider());
            base.OnConfiguring(optionsBuilder_.EnableSensitiveDataLogging().UseLoggerFactory(loggerFactory));
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder_)
        {
            Console.WriteLine("ApplicationStorageContext: OnModelCreating");
            base.OnModelCreating(modelBuilder_);
            this.RegisterEntities(modelBuilder_);
        }

    }
}

