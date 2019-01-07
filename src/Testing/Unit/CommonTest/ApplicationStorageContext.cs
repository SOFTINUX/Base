// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Infrastructure.Util;
using SoftinuxBase.Security.Data.Entities;

namespace CommonTest
{
    public class ApplicationStorageContext : IdentityDbContext<User, IdentityRole<string>, string>, IStorageContext
    {
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }

        public ApplicationStorageContext(DbContextOptions<ApplicationStorageContext> options_)
            : base(options_)
        {
            Utilities.LoadExtensions();
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