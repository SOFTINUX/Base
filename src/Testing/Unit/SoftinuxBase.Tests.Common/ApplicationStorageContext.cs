// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.RefreshClaims;

namespace SoftinuxBase.Tests.Common
{
    /// <summary>
    /// Inherit from SoftinuxBase regular application Db Context so that we can create migrations here in the test project.
    /// </summary>
    public class ApplicationStorageContext : SoftinuxBase.Security.Data.EntityFramework.ApplicationStorageContext
    {
        public ApplicationStorageContext(DbContextOptions options_)
            : base(options_)
        {
        }

        protected ApplicationStorageContext(DbContextOptions options_, IAuthChanges authChange_)
            : base(options_)
        {
            _authChange = authChange_;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder_)
        {
        }
    }
}