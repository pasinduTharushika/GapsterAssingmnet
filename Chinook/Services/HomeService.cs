
using Chinook.ClientModels;
using Chinook.Interface;
using Chinook.Interfaces;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Service
{
    public class HomeService : IHomeService
    {
        private readonly ChinookContext _context;

        public HomeService(ChinookContext context)
        {
            _context = context;
        }

        public async Task<List<ArtistDTO>> GetArtists()
        {
            try
            {
                return await _context.Artists.Include(a => a.Albums).Select(a => new ArtistDTO()
                {
                    Name = a.Name,
                    ArtistId = a.ArtistId,
                    AlbumsCount=a.Albums.Count()
                    
                }).ToListAsync();

            }
            catch (Exception ex)
            {
                return new List<ArtistDTO>();
            }

            


        }

        public async Task<List<AlbumDTO>> GetAlbumsForArtist(int artistId)
        {
            try
            {
                return await _context.Albums.Where(a => a.ArtistId == artistId)
                    .Select(a => new AlbumDTO()
                    {
                        Title = a.Title,
                        ArtistId = a.ArtistId,
                        AlbumId = a.AlbumId

                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<AlbumDTO>();
            }
            
        }

        public async Task<List<ArtistDTO>> GetArtistsBySearch(string searchTerm)
        {
            try
            {
                return await _context.Artists.Where(a => a.Name.StartsWith(searchTerm)).Include(a => a.Albums).Select(a => new ArtistDTO()
                {
                    Name = a.Name,
                    ArtistId = a.ArtistId,
                    AlbumsCount = a.Albums.Count()

                }).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<ArtistDTO>();
            }
            
        }

    }
}
