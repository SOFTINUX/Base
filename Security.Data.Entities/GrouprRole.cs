using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between groups and roles.
    /// </summary>
    public class GroupRole : IEntity
    {
        public int GroupId { get; set; }

        public int RoleId { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual Group Group { get; set; }
        public virtual Role Role { get; set; }
    }
}
