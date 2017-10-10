using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    public class Group : IEntity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Label { get; set; }

        /// <summary>
        /// Full name of extension's assembly, to manage data by extension (add, reset, remove).
        /// </summary>
        public string OriginExtension { get; set; }
    }
}
