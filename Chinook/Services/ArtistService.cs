using Chinook.ClientModels;
using Chinook.Interface;
using Chinook.Models;
using Chinook.Pages;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NuGet.DependencyResolver;
using System.Linq;
using Playlist = Chinook.Models.Playlist;

namespace Chinook.Service
{
    public class ArtistService : IArtistService
    {
        private readonly ChinookContext _context;
        private readonly IConfiguration _configuration;
        public ArtistService(ChinookContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ArtistDTO> GetArtistById(long ArtistId)
        {
            try
            {
                return await _context.Artists.Where(a => a.ArtistId == ArtistId).Select(a => new ArtistDTO()
                {
                    Name = a.Name,
                    ArtistId = a.ArtistId,
                    AlbumsCount = a.Albums.Count()

                }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return new ArtistDTO();
            }
            

        }

        public async Task<List<PlaylistTrackDTO>> GetTracksByArtistId(long ArtistId, string CurrentUserId)
        {
            try
            {
                string favoritsListName = _configuration["MyFavoritetracks"];


                return await _context.Tracks.Where(a => a.Album.ArtistId == ArtistId)
                .Include(a => a.Album)
                .Select(t => new ClientModels.PlaylistTrackDTO()
                {
                    AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                    TrackId = t.TrackId,
                    TrackName = t.Name,
                   
                    IsFavorite = t.PlaylistTracks.Where(a => a.TrackId == t.TrackId && a.Playlist.UserPlaylists.Any(a => a.UserId.Contains(CurrentUserId))).Select(a => a.Playlist.Name).FirstOrDefault() == favoritsListName ? true : false //t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == CurrentUserId && up.Playlist.Name == favoritsListName)).Any()
                }).ToListAsync();

            }
            catch (Exception ex)
            {
                return new List<PlaylistTrackDTO>();

            }
           



        }
        public async Task<List<PlaylistDTO>> GetPlaylists()
        {
            try
            {
                return await _context.Playlists.Select(a => new PlaylistDTO()
                {
                    Name= a.Name,
                    PlaylistId=a.PlaylistId
                    
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<PlaylistDTO>();
            }
            
        }

        public async Task<string> AddTrackToPlaylist(string SelectedOption, long trackId,string CurrentUserId)
        {
            try
            {
                await AddUserPlayList(CurrentUserId, int.Parse(SelectedOption));
                Models.PlaylistTrack playList = new Models.PlaylistTrack();
                playList.TrackId = trackId;
                playList.PlaylistId = int.Parse(SelectedOption);
                _context.PlaylistTracks.Add(playList);
                await _context.SaveChangesAsync();
                return "Add Track to PlayList Successfully";
            }
            catch (Exception ex)
            {
                return "Add Track to PlayList Unsuccessfully";
            }
            


        }

        public async Task<string> AddNewPlaylist(string newPlaylistValue, string SelectedOption, long trackId, string CurrentUserId)
        {
            try
            {
                Playlist playlist =  await _context.Playlists.AsNoTracking().Where( a => a.Name == newPlaylistValue).FirstOrDefaultAsync();
                if (playlist == null)
                {
                    
                    Models.Playlist playList = new Models.Playlist();
                    playList.Name = newPlaylistValue;
                    _context.Playlists.Add(playList);
                    await _context.SaveChangesAsync();
                    string outPut = await AddTrackToPlaylist(playList.PlaylistId.ToString(), trackId, CurrentUserId);
                    return "Add new PlayList Successfully";
                }
                else
                {
                    
                    return "Add new PlayList Unsuccessfully";
                }

                
               

            }
            catch(Exception ex)
            {
                return "Add new PlayList Unsuccessfully";
            }
           

        }

        public async Task<string> AddandremoveFavoriteTrack(long trackId, string CurrentUserId, bool IsAdd)
        {
            string favoritsListName = _configuration["MyFavoritetracks"];
            Playlist playlist = await _context.Playlists.AsNoTracking().Where(a => a.Name == favoritsListName).FirstOrDefaultAsync();
            if (IsAdd)
            {
               
                if (playlist == null)
                {
                    try
                    {
                        Models.Playlist playList = new Models.Playlist();
                        playList.Name = favoritsListName;
                        _context.Playlists.Add(playList);
                        await _context.SaveChangesAsync();
                        string outPut = await AddTrackToPlaylist(playList.PlaylistId.ToString(), trackId, CurrentUserId);
                        return "Add  FavoriteTrack Successfully ";
                    }
                    catch (Exception ex)
                    {
                        return "Add  FavoriteTrack Unuccessfully ";
                    }
                   
                }
                else
                {
                    string outPut = await AddTrackToPlaylist(playlist.PlaylistId.ToString(),  trackId, CurrentUserId);
                    return "Add Track to PlayList Successfully";
                }

            }
            else
            {
                try
                {
                    Models.PlaylistTrack playList = await _context.PlaylistTracks.AsNoTracking().Where(a => a.TrackId == trackId && a.PlaylistId == playlist.PlaylistId).FirstOrDefaultAsync();

                    if (playList == null)
                    {
                        return "";
                    }

                    _context.PlaylistTracks.Remove(playList);
                    await _context.SaveChangesAsync();
                    return "Remove  FavoriteTrack Successfully ";

                }
                catch(Exception ex)
                {
                    return "Remove  FavoriteTrack Unsuccessfully ";
                }
                


            }
           

        }

        private async Task<string> AddUserPlayList(string userId,long playlistId)
        {
            try
            {
                UserPlaylist userPlaylist = _context.UserPlaylists.AsTracking().Where(a => a.UserId == userId && a.PlaylistId == playlistId).FirstOrDefault();
                if (userPlaylist == null)
                {
                    Models.UserPlaylist userplaylist = new Models.UserPlaylist();
                    userplaylist.UserId = userId;
                    userplaylist.PlaylistId = playlistId;
                    _context.UserPlaylists.Add(userplaylist);
                    await _context.SaveChangesAsync();

                    return "Add User PlayList Successfully";
                }
                else
                {
                    return "Allready Add User PlayList Successfully";
                }
            }
            catch (Exception ex)
            {
                return "Add User PlayList Unsuccessfully";
            }
           
        }
    }
}
