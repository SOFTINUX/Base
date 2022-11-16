#pragma warning disable SA1636
/* Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
 Licensed under MIT license. See License.txt in the project root for license information. */

using System;
using System.ComponentModel.DataAnnotations;
using SoftinuxBase.Security.Permissions;

namespace SoftinuxBase.Security.Data.Entities
{
    /// <summary>
    /// This holds what modules a user or tenant has.
    /// </summary>
    public class ModulesForUser : IChangeEffectsUser, IAddRemoveEffectsUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModulesForUser"/> class.
        /// This links modules to a user.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ModulesForUser"/> class.
        /// Empty constructor for migration to work.
        /// </summary>
#pragma warning disable SA1201

        // ReSharper disable once UnusedMember.Local
        private ModulesForUser()
        {
        }
#pragma warning restore SA1201
    }
}