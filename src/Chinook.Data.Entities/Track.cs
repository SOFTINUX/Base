// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using ExtCore.Data.Entities.Abstractions;

namespace Chinook.Data.Entities
{
    [DebuggerDisplay("{Name} (TrackId = {TrackId})")]
    public class Track : IEntity
    {
        [Key] public int TrackId { get; set; }

        [Required, MaxLength(200)] public string Name { get; set; }

        public int AlbumId { get; set; }

        public int MediaTypeId { get; set; }

        public int GenreId { get; set; }

        [MaxLength(220)] public string Composer { get; set; }

        public int Miliseconds { get; set; }

        public int Bytes { get; set; }

        public decimal UnitPrice { get; set; }

        [ForeignKey("AlbumId")] public Album Album { get; set; }

        [ForeignKey("MediaTypeId")] public MediaType MediaType { get; set; }

        [ForeignKey("GenreId")] public Genre Genre { get; set; }
    }
}
