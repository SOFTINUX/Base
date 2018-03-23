// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using ExtCore.Data.Entities.Abstractions;

namespace Chinook.Data.Entities
{
    [DebuggerDisplay("{Title} (AlbumId = {AlbumId}")]
    public class Album : IEntity
    {
        [Key]
        public int AlbumId { get; set; }

        [Required, MaxLength(160)]
        public string Title { get; set; }

        [Required]
        public int ArtistId {get; set; }

        [ForeignKey("ArtistId")]
        public Artist artist { get; set; }
    }
}
