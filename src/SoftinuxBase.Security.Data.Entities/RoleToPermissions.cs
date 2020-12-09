#pragma warning disable SA1636
/* Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX.
 Licensed under MIT license. See License.txt in the project root for license information. */

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ExtCore.Data.Entities.Abstractions;
using SoftinuxBase.Security.Permissions;

[assembly: InternalsVisibleTo("SoftinuxBase.Security")]

namespace SoftinuxBase.Security.Data.Entities
{
    /// <summary>
    /// This holds each Roles, which are mapped to Permissions.
    /// </summary>
    public class RoleToPermissions : IChangeEffectsUser, IEntity
    {
        // Value stored in database that represents permissions.
        [Required(AllowEmptyStrings = false)] // A role must have at least one permission in it
        private string _permissionsInRole;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleToPermissions"/> class.
        /// This creates the Role with its permissions.
        /// </summary>
        /// <param name="roleName_"></param>
        /// <param name="description_"></param>
        /// <param name="permissions_"></param>
        public RoleToPermissions(string roleName_, string description_, PermissionsDictionary permissions_)
        {
            RoleName = roleName_;
            Description = description_;
            _permissionsInRole = permissions_.PackPermissions().ToStorageString();
        }

        private RoleToPermissions()
        {
        }

        /// <summary>
        /// Gets shortName of the role.
        /// </summary>
        [Key]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(ExtraAuthConstants.RoleNameSize)]

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - needed by EF Core
        public string RoleName { get; internal set; }

        /// <summary>
        /// Gets a human-friendly description of what the Role does.
        /// </summary>
        [Required(AllowEmptyStrings = false)]

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - needed by EF Core
        public string Description { get; private set; }

        /// <summary>
        /// Gets the set of permissions in this role.
        /// Intended for reading. To set permissions, use <see cref="Update"/>.
        /// </summary>
        public PermissionsDictionary PermissionsForRole => _permissionsInRole.ToPermissions();

        public void Update(PermissionsDictionary permissions_)
        {
             _permissionsInRole = permissions_.PackPermissions().ToStorageString();
        }

        /// <summary>
        /// Creates a (transient) copy of current, with a new role name.
        /// </summary>
        /// <param name="newRoleName_">New role name.</param>
        public RoleToPermissions Copy(string newRoleName_) {
            return new RoleToPermissions {
                RoleName = newRoleName_,
                _permissionsInRole = _permissionsInRole,
                Description = newRoleName_,
            };
        }
    }
}