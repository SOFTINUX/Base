
using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    public class PermissionLevel : IEntity
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public int Label { get; set; }

        public int Tip { get; set; }
    }
}
