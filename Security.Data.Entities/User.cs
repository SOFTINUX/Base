using System;
using System.Collections.Generic;
using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Represents an user base information and linked credentials.
    /// </summary>
    public class User : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public DateTime FirstConnection { get; set; }
        public DateTime LastConnection { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual ICollection<Credential> Credentials { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
