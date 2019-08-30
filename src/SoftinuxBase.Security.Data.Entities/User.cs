// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Data.Entities.Abstractions;
using Microsoft.AspNetCore.Identity;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.Abstractions")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.EntityFramework")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.WebApplication")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SecurityTest")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("CommonTest")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SeedDatabase")]
namespace SoftinuxBase.Security.Data.Entities
{
    public class User : IdentityUser, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime FirstConnection { get; set; }
        public DateTime LastConnection { get; set; }
    }
}
