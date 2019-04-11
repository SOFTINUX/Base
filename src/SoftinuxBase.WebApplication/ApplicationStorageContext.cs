// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.WebApplication
{
    /// <summary>
    /// Class that holds the Entity Framework DbContext's DbSets related to some extension
    /// (entities in XXX.Data.Entities project) and that also inherits from ExtCore's IStorageContext.
    /// </summary>
    public class ApplicationStorageContext : IdentityDbContext<User, IdentityRole<string>, string>, IStorageContext
    {
        private readonly ILoggerFactory _loggerFactory;

        protected ApplicationStorageContext(DbContextOptions options_)
            : base(options_)
        {
        }

        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder_)
        {
#if DEBUG
            //ILoggerFactory loggerFactory = new LoggerFactory();
            //_loggerFactory.AddProvider(new EfLoggerProvider());
            base.OnConfiguring(optionsBuilder_.EnableSensitiveDataLogging() /*.UseLoggerFactory(_loggerFactory)*/);
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder_)
        {
            base.OnModelCreating(modelBuilder_);
            this.RegisterEntities(modelBuilder_);
        }
    }
}