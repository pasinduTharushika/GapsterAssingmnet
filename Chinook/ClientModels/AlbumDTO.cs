namespace Chinook.ClientModels
{
    public class AlbumDTO
    {
        public long AlbumId { get; set; }
        public string Title { get; set; } = null!;
        public long ArtistId { get; set; }
    }
}
