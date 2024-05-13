using Chinook.ClientModels;

namespace Chinook.Interfaces
{
    public interface IPlaylistService
    {
        Task<List<PlaylistDTO>> GetPlaylists();
        Task<PlaylistDTO> LoadPlaylistDetails(int selectedValue, string CurrentUserId);
        Task<string> RemoveTrack(long trackId,int selectedValue);
    }
}
