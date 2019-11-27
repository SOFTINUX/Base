// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ExtCore.Data.Entities.Abstractions;
using GenericServices;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses.Support;
using SoftinuxBase.Security.PermissionParts;

namespace SoftinuxBase.Security.DataLayer.ExtraAuthClasses
{
    /// <summary>
    /// This holds each Roles, which are mapped to Permissions
    /// </summary>
    public class RoleToPermissions : IChangeEffectsUser, IEntity
    {
        [Required(AllowEmptyStrings = false)] //A role must have at least one role in it
        private string _permissionsInRole;

        private RoleToPermissions() { }

        /// <summary>
        /// This creates the Role with its permissions
        /// </summary>
        /// <param name="roleName_"></param>
        /// <param name="description_"></param>
        /// <param name="permissions_"></param>
        public RoleToPermissions(string roleName_, string description_, ICollection<Permissions> permissions_)
        {
            RoleName = roleName_;
            Update(description_, permissions_);
        }

        /// <summary>
        /// ShortName of the role
        /// </summary>
        [Key]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(ExtraAuthConstants.RoleNameSize)]
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - needed by EF Core
        public string RoleName { get; private set; }

        /// <summary>
        /// A human-friendly description of what the Role does
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - needed by EF Core
        public string Description { get; private set; }

        /// <summary>
        /// This returns the list of permissions in this role
        /// </summary>
        public IEnumerable<Permissions> PermissionsInRole => _permissionsInRole.UnpackPermissionsFromString();

        public void Update(string description_, ICollection<Permissions> permissions_)
        {
            if (permissions_ == null || !permissions_.Any())
                throw new ArgumentException("There should be at least one permission associated with a role.", nameof(permissions_));

            _permissionsInRole = permissions_.PackPermissionsIntoString();
            Description = description_;
        }

        public IStatusGeneric DeleteRole(string roleName_, bool removeFromUsers_,
            ApplicationStorageContext context_)
        {
            var status = new StatusGenericHandler { Message = "Deleted role successfully." };
            var roleToUpdate = context_.Find<RoleToPermissions>(roleName_);
            if (roleToUpdate == null)
                return status.AddError("That role doesn't exists");

            var usersWithRoles = context_.UserToRoles.Where(x_ => x_.RoleName == roleName_).ToList();
            if (usersWithRoles.Any())
            {
                if (!removeFromUsers_)
                    return status.AddError($"That role is used by {usersWithRoles.Count} and you didn't ask for them to be updated.");

                context_.RemoveRange(usersWithRoles);
                status.Message = $"Removed role from {usersWithRoles.Count} user and then deleted role successfully.";
            }

            context_.Remove(roleToUpdate);
            return status;
        }
    }
}