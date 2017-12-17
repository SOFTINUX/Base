using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Security.Data.Entities;

namespace WebApplication
{
    public class ApplicationStorageContext : IdentityDbContext<User, Role, int>, IStorageContext
    {
        /// <summary>
        /// The connection string that is used to connect to the physical storage.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// The assembly name where migrations are maintained for this context.
        /// </summary>
        public string MigrationsAssembly { get; }

        public ApplicationStorageContext(IOptions<StorageContextOptions> options_)
        {
            ConnectionString = options_.Value.ConnectionString;
            MigrationsAssembly = options_.Value.MigrationsAssembly;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder_)
        {
            optionsBuilder_.UseSqlite(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.RegisterEntities(modelBuilder);
        }

    }
}

