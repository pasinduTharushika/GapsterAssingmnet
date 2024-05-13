namespace Chinook.ClientModels;

public class PlaylistDTO
{
    public long PlaylistId { get; set; }
    public string Name { get; set; }
    public List<PlaylistTrackDTO> Tracks { get; set; }
}