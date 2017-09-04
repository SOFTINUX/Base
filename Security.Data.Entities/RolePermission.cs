using ExtCore.Data.Entities.Abstractions;
using Microsoft.Net.Http.Headers;

namespace Security.Data.Entities
{
    /// <summary>
    /// Links between roles and permissions. Also stores the permission level.
    /// </summary>
    public class RolePermission : IEntity
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public PermissionLevel PermissionLevel { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
