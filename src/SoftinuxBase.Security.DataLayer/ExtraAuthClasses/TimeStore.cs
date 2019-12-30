using System.ComponentModel.DataAnnotations;
using SoftinuxBase.Security.RefreshClaimsParts;

namespace SoftinuxBase.Security.DataLayer.ExtraAuthClasses
{
    [NoQueryFilterNeeded]
    public class TimeStore
    {
        [Key]
        [Required]
        [MaxLength(AuthChangesConsts.CacheKeyMaxSize)]
        public string Key { get; set; }

        public long LastUpdatedTicks { get; set; }
    }
}