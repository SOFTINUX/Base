using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between users and roles.
    /// </summary>
    public class UserRole : IEntity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
