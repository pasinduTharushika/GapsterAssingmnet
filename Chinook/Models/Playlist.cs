﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chinook.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            Tracks = new HashSet<Track>();
        }
        [Key]
        public long PlaylistId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
        public virtual ICollection<UserPlaylist> UserPlaylists { get; set; }
        public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; }

    }
}
