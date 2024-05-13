using Chinook.ClientModels;
using Chinook.Interfaces;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Chinook.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ChinookContext _context;
        private readonly IConfiguration _configuration;
        public PlaylistService(ChinookContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<List<PlaylistDTO>> GetPlaylists()
        {
            try
            {
                return await _context.Playlists.Select(a => new PlaylistDTO()
                {
                    Name=a.Name,
                    PlaylistId = a.PlaylistId,

                }).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<PlaylistDTO>();
            }
           
        }
        public async Task<PlaylistDTO> LoadPlaylistDetails(int selectedValue, string CurrentUserId)
        {
            try
            {
                string favoritsListName = _configuration["MyFavoritetracks"];
                return await _context.Playlists.AsNoTracking()
                    .Where(a => a.PlaylistId == selectedValue)
                    .Select(a => new ClientModels.PlaylistDTO()
                    {
                        Name = a.Name,
                        Tracks = a.PlaylistTracks.Select(t => new ClientModels.PlaylistTrackDTO()
                        {
                            AlbumTitle = t.Track.Album.Title,
                            ArtistName = t.Track.Album.Artist.Name,
                            TrackId = t.TrackId,
                            TrackName = t.Track.Name,
                            IsFavorite = t.Playlist.Name == favoritsListName ? true : false,
                        }).ToList()
                    }).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                return new PlaylistDTO();
            }
           
                

        }

        public async Task<string> RemoveTrack(long trackId, int selectedValue)
        {
            try
            {
                Models.PlaylistTrack playList = await _context.PlaylistTracks.AsNoTracking().Where(a => a.TrackId == trackId && a.PlaylistId == selectedValue).FirstOrDefaultAsync();

                if (playList == null)
                {
                    return "Remove Track Unsuccessfully";
                }
               
                _context.PlaylistTracks.Remove(playList);
                await _context.SaveChangesAsync();
                return "Remove Track Successfully";

            }
            catch (Exception ex)
            {
                return "Remove Track Unsuccessfully";
            }
        }
    }
}
