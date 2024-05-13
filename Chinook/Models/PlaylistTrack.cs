using System.ComponentModel.DataAnnotations;

namespace Chinook.Models
{
    public class PlaylistTrack
    {
        [Key]
        public int PlaylistTrackId { get; set; }
        public long PlaylistId { get; set; }
        public long TrackId { get; set; }

        public Playlist Playlist { get; set; }
        public Track Track { get; set; }
    }
}
