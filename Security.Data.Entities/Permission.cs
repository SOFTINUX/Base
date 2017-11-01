using System.Collections.Generic;
using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    public class Permission : IEntity
    {       
        public int Id { get; set; }

        public string Code { get; set; }

        public string Label { get; set; }

        public bool AdministratorOwner { get; set; }

        /// <summary>
        /// Full name of extension's assembly, to manage data by extension (add, reset, remove).
        /// </summary>
        public string OriginExtension { get; set; }

        /// <summary>
        /// Unique identifier : code + origin extension name.
        /// </summary>
        public string UniqueIdentifier => $"{Code}|{OriginExtension}";

        /// <summary>
        /// Referenced entities, here the link table because it has other data to store than just FKs.
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
