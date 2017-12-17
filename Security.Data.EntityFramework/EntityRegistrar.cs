// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder_)
        {

            /*  modelBuilder_.Entity<UserLogin>(etb_ => {
                 etb_.HasKey(e_ => new { e_.ProviderKey, e_.LoginProvider });
             });

             modelBuilder_.Entity<User>(etb_ => {
                 etb_.HasKey(e_ => new { e_.Id });
             });

             modelBuilder_.Entity<Role>(etb_ => {
                 etb_.HasKey(e_ => new { e_.Id });
             });

             modelBuilder_.Entity<UserRole>(etb_ => {
                 etb_.HasKey(e_ => new { e_.UserId , e_.RoleId});
             });

             modelBuilder_.Entity<UserToken>(etb_ => {
                 etb_.HasKey(e_ => new { e_.LoginProvider, e_.Name });
             }); */

            // Redefinition so that there are no extra FK/columns created
            // https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x
            modelBuilder_.Entity<User>()
                .HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder_.Entity<User>()
                .HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder_.Entity<Role>()
                .HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder_.Entity<User>()
                .HasMany(e => e.UserLogins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder_.Entity<User>()
                .HasMany(e => e.UserTokens)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // non-WIF entities

            // Permission
            modelBuilder_.Entity<Permission>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // RolePermission
            modelBuilder_.Entity<RolePermission>(etb_ =>
                {
                    etb_.HasKey(e_ => new { e_.RoleId, e_.PermissionId });
                }
            );

            // Group
            modelBuilder_.Entity<Group>(etb_ =>
                {
                    etb_.HasKey(e_ => e_.Id);
                    etb_.Property(e_ => e_.Id).ValueGeneratedOnAdd();
                }
            );

            // UserGroup
            modelBuilder_.Entity<UserGroup>(etb_ =>
                {
                    etb_.HasKey(e_ => new { e_.GroupId, e_.UserId });
                }
            );

            // GroupPermission
            modelBuilder_.Entity<GroupPermission>(etb_ =>
            {
                etb_.HasKey(e_ => new { e_.GroupId, e_.PermissionId });
            }
            );

            // UserPermission
            modelBuilder_.Entity<UserPermission>(etb_ =>
                {
                    etb_.HasKey(e_ => new { e_.UserId, e_.PermissionId });
                }
            );
        }
    }
}
