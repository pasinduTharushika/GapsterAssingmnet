using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Interface
{
    public interface IHomeService
    {
        Task<List<ArtistDTO>> GetArtists();
        Task<List<AlbumDTO>> GetAlbumsForArtist(int artistId);
        Task<List<ArtistDTO>> GetArtistsBySearch(string searchTerm);
    }
}
