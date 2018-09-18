// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using ExtCore.Data.Entities.Abstractions;

namespace Chinook.Data.Entities
{
    [DebuggerDisplay("{Name} (MediaTypeId = {MediaTypeId})")]
    public class MediaType : IEntity
    {
        [Key]
        public int MediaTypeId { get; set; }

        [MaxLength(120)]
        public string Name { get; set; }
    }
}
