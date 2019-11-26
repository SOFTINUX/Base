// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses.Support;
using SoftinuxBase.Security.PermissionParts;

namespace SoftinuxBase.Security.DataLayer.ExtraAuthClasses
{
    /// <summary>
    /// This holds what modules a user or tenant has
    /// </summary>
    public class ModulesForUser : IChangeEffectsUser, IAddRemoveEffectsUser
    {
        /// <summary>
        /// Empty constructor for migration to work.
        /// </summary>
        private ModulesForUser() { }

        /// <summary>
        /// This links modules to a user
        /// </summary>
        /// <param name="userId_"></param>
        /// <param name="allowedPaidForModules_"></param>
        public ModulesForUser(string userId_, PaidForModules allowedPaidForModules_)
        {
            UserId = userId_ ?? throw new ArgumentNullException(nameof(userId_));
            AllowedPaidForModules = allowedPaidForModules_;
        }

        [Key]
        [MaxLength(ExtraAuthConstants.UserIdSize)]
        public string UserId { get; private set; }

        public PaidForModules AllowedPaidForModules { get; private set; }
    }
}