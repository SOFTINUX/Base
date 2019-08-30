// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;
using ExtCore.Data.Entities.Abstractions;
using SoftinuxBase.Security.Common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.Abstractions")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.Security.Data.EntityFramework")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.WebApplication")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SoftinuxBase.SeedDatabase")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("CommonTest")]
namespace SoftinuxBase.Security.Data.Entities
{
    /// <summary>
    /// Links between users and permissions: permissions assigned to the user.
    /// </summary>
    public class UserPermission : IEntity
    {
        public UserPermission()
        {
            Extension = Constants.SoftinuxBaseSecurity;
        }

        public string UserId { get; set; }
        public string PermissionId { get; set; }
        public string Extension { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}
