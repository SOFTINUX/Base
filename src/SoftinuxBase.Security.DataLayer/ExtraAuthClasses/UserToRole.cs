// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericServices;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses.Support;

namespace SoftinuxBase.Security.DataLayer.ExtraAuthClasses
{
    /// <summary>
    /// This is a one-to-many relationship between the User (represented by the UserId) and their Roles (represented by RoleToPermissions)
    /// </summary>
    public class UserToRole : IAddRemoveEffectsUser, IChangeEffectsUser
    {
        private UserToRole() { } //needed by EF Core

        public UserToRole(string userId_, RoleToPermissions role_)
        {
            UserId = userId_;
            Role = role_;
        }

        //I use a composite key for this table: combination of UserId and RoleName
        //That has to be defined by EF Core's fluent API
        [Required(AllowEmptyStrings = false)]
        [MaxLength(ExtraAuthConstants.UserIdSize)]
        public string UserId { get; private set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(ExtraAuthConstants.RoleNameSize)]
        public string RoleName { get; private set; }

        [ForeignKey(nameof(RoleName))]
        public RoleToPermissions Role { get; private set; }


        public static IStatusGeneric<UserToRole> AddRoleToUser(string userId_, string roleName_, ApplicationStorageContext context_)
        {
            if (roleName_ == null) throw new ArgumentNullException(nameof(roleName_));

            var status = new StatusGenericHandler<UserToRole>();
            if (context_.Find<UserToRole>(userId_, roleName_) != null)
            {
                status.AddError($"The user already has the Role '{roleName_}'.");
                return status;
            }
            var roleToAdd = context_.Find<RoleToPermissions>(roleName_);
            if (roleToAdd == null)
            {
                status.AddError($"I could not find the Role '{roleName_}'.");
                return status;
            }

            return status.SetResult(new UserToRole(userId_, roleToAdd));
        }
    }
}