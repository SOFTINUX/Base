
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder_)
        {
            // CredentialType
            modelBuilder_.Entity<CredentialType>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // Credential
            modelBuilder_.Entity<Credential>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // Permission
            modelBuilder_.Entity<Permission>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // Role
            modelBuilder_.Entity<Role>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // RolePermission
            modelBuilder_.Entity<RolePermission>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // User
            modelBuilder_.Entity<User>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // UserRole
            modelBuilder_.Entity<UserRole>(etb_ =>
                {
                    etb_.HasKey(e_ => new { e_.RoleId, e_.UserId });
                }
            );

            // PermissionLevel
            modelBuilder_.Entity<PermissionLevel>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // Group
            modelBuilder_.Entity<Group>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // GroupUser
            modelBuilder_.Entity<GroupUser>(etb_ =>
                {
                    etb_.HasKey(e_ => new { e_.GroupId, e_.UserId });
                }
            );

            // GroupPermission
            modelBuilder_.Entity<GroupPermission>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // UserPermission
            modelBuilder_.Entity<UserPermission>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );
        }
    }
}
