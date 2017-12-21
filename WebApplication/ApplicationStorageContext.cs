using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Security.Data.Entities;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.RegisterEntities(modelBuilder);
        }

    }
}

