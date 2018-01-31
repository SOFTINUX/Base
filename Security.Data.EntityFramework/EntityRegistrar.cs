// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder_)
        {

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
