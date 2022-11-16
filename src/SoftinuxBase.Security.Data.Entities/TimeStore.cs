// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;
using SoftinuxBase.Security.RefreshClaims;

namespace SoftinuxBase.Security.Data.Entities
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