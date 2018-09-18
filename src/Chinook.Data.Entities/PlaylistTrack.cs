// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using ExtCore.Data.Entities.Abstractions;

namespace Chinook.Data.Entities
{
    [DebuggerDisplay("PlaylistId = {PlaylistId}, TrackId = {TrackId}")]
    public class PlaylistTrack : IEntity
    {
        [Key, Column(Order=1)]
        public int PlaylistId { get; set; }

        [Key, Column(Order = 2)]
        public int TrackId { get; set; }

        [ForeignKey("PlaylistId")]
        public Playlist Playlist { get; set; }

        [ForeignKey("TrackId")]
        public Track Track { get; set; }
    }
}
