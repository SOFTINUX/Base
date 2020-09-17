#pragma warning disable SA1636
#pragma warning disable SA1515
/* Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
 Licensed under MIT license. See License.txt in the project root for license information. */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExtCore.Data.Entities.Abstractions;

namespace SoftinuxBase.Security.Data.Entities
{
    /// <summary>
    /// This is a one-to-many relationship between the User (represented by the UserId) and their Roles (represented by RoleToPermissions).
    /// </summary>
    public class UserToRole : IAddRemoveEffectsUser, IChangeEffectsUser, IEntity
    {
        // needed by EF Core
        // ReSharper disable once UnusedMember.Local
        private UserToRole()
        {
        }

        public UserToRole(string userId_, RoleToPermissions role_)
        {
            UserId = userId_;
            Role = role_;
        }

        // I use a composite key for this table: combination of UserId and RoleName
        // That has to be defined by EF Core's fluent API
        [Required(AllowEmptyStrings = false)]
        [MaxLength(ExtraAuthConstants.UserIdSize)]
        // ReSharper disable once SA1515
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - needed by EF Core
        public string UserId { get; private set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(ExtraAuthConstants.RoleNameSize)]
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - needed by EF Core
        public string RoleName { get; private set; }

        [ForeignKey(nameof(RoleName))]
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - needed by EF Core
        public RoleToPermissions Role { get; private set; }
    }
}