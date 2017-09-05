using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between groups and users.
    /// </summary>
    public class GroupUser : IEntity
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
    }
}
