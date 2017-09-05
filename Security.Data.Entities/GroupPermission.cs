using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between groups and permissions. Also stores the permission level.
    /// </summary>
    public class GroupPermission : IEntity
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int PermissionId { get; set; }

        public int PermissionLevelId { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual Group Group { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual PermissionLevel PermissionLevel { get; set; }

    }
}
