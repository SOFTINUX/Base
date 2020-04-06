// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using Microsoft.EntityFrameworkCore;

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
    }
}