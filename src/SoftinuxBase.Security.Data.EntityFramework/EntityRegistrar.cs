// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder_)
        {
            // Class provided by Identity

            modelBuilder_.Entity<IdentityRole>()
                .Property(e_ => e_.Id)
                .ValueGeneratedOnAdd();
        }
    }
}