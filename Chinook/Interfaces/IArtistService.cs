using Chinook.ClientModels;
using Chinook.Models;
using NuGet.DependencyResolver;

namespace Chinook.Interface
{
    public interface IArtistService
    {
        Task<ArtistDTO> GetArtistById(long ArtistId);
        Task<List<PlaylistTrackDTO>> GetTracksByArtistId(long ArtistId, string CurrentUserId);
        Task<List<PlaylistDTO>> GetPlaylists();
        Task<string> AddTrackToPlaylist(string SelectedOption, long trackId,string CurrentUserId);
        Task<string> AddNewPlaylist(string newPlaylistValue,string SelectedOption, long trackId,string CurrentUserId);
        Task<string> AddandremoveFavoriteTrack(long trackId,string CurrentUserId, bool IsAdd);


    }
}
