using Microsoft.EntityFrameworkCore;
using SonicSpectrum.Application.DTOs;
using SonicSpectrum.Application.Repository.Abstract;
using SonicSpectrum.Application.Services;
using SonicSpectrum.Domain.Entities;
using SonicSpectrum.Persistence.Data;

namespace SonicSpectrum.Application.Repository.Concrete
{
    public class MusicSettingService(AppDbContext _context) : IMusicSettingService
    {
        #region Add

        public async Task<bool> AddAlbumAsync(AlbumDto albumDto)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(art=>art.Id == albumDto.ArtistId);
            if (artist != null)
            {
                var album = new Album { Title = albumDto.Title, ArtistId = artist.Id };
                await _context.Albums.AddAsync(album);
                await _context.SaveChangesAsync();
                return true;
            }
            await Console.Out.WriteLineAsync($"{artist} is don't find");
            return false;
        }

        public async Task<bool> AddArtistAsync(ArtistDTO artistDto)
        {
            if(artistDto.Name != null)
            {
                var artist = new Artist { Name = artistDto.Name! };
                await _context.Artists.AddAsync(artist);
                await _context.SaveChangesAsync();
                return true;
            }
            await Console.Out.WriteLineAsync($"{artistDto.Name} is null");
            return false;
        }

        public async Task<bool> AddGenreAsync(GenreDTO genreDto)
        {
            var genreName = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreDto.Name);
            if (genreName == null)
            {
                var genre = new Genre { Name = genreDto.Name! };
                await _context.Genres.AddAsync(genre);
                await _context.SaveChangesAsync();
                return true;
            }
            await Console.Out.WriteLineAsync($"{genreName} is already extist");
            return false;
        }

        public async Task<bool> AddTrackAsync(TrackDTO trackDto)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.Id == trackDto.ArtistId);
            if (artist == null)
            {
                await Console.Out.WriteLineAsync($"Artist with ID {trackDto.ArtistId} is not found");
                return false;
            }

            var track = new Track
            {
                Title = trackDto.Title!,
                ArtistId = trackDto.ArtistId!,
                AlbumId = trackDto.AlbumId!,
                FilePath = await UploadFileHelper.UploadFile(trackDto.FilePath!, "musicplay", trackDto.Title!),
                ImagePath = await UploadFileHelper.UploadFile(trackDto.ImagePath!, "musicphoto", trackDto.Title!)
            };

            track.Albums = new HashSet<Album>();
            track.Genres = new HashSet<Genre>();
            track.Lyrics = new HashSet<Lyric>();

            if (trackDto.AlbumTitles != null)
            {
                foreach (var albumTitle in trackDto.AlbumTitles)
                {
                    var album = await _context.Albums.FirstOrDefaultAsync(a => a.Title == albumTitle);
                    if (album != null)
                        track.Albums.Add(album);
                }
            }

            if (trackDto.GenreNames != null)
            {
                foreach (var genreName in trackDto.GenreNames)
                {
                    var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                    if (genre != null)
                        track.Genres.Add(genre);
                }
            }

            if (trackDto.Lyrics != null)
            {
                foreach (var lyricDto in trackDto.Lyrics)
                {
                    string lyricText = lyricDto.Text!;
                    var lyric = new Lyric { Text = lyricText };
                    track.Lyrics.Add(lyric);
                }
            }

            await _context.Tracks.AddAsync(track);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                await Console.Out.WriteLineAsync($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"An error occurred: {ex.Message}");
            }
            return true;
        }

        #endregion

        #region Delete

        public async Task<bool> DeleteArtistAsync(string artistId)
        {
            var artist = await _context.Artists.FindAsync(artistId);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
                return true;
            }
            await Console.Out.WriteLineAsync($"{artist} is null");
            return false;
        }

        public async Task<bool> DeleteAlbumAsync(string albumId)
        {
            var album = await _context.Albums.FindAsync(albumId);
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
                return true;
            }
            await Console.Out.WriteLineAsync($"Album with ID {albumId} is not found");
            return false;
        }

        public async Task<bool> DeleteGenreAsync(string genreId)
        {
            var genre = await _context.Genres.FindAsync(genreId);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
                return true;
            }
            await Console.Out.WriteLineAsync($"{genre} is null");
            return false;
        }

        public async Task<bool> DeleteTrackAsync(string trackId)
        {
            var track = await _context.Tracks.FindAsync(trackId);
            if (track != null)
            {
                _context.Tracks.Remove(track);
                await _context.SaveChangesAsync();

                await UploadFileHelper.DeleteFile(track.Title, "musicplay");
                await UploadFileHelper.DeleteFile(track.Title, "musicphoto");
                await Console.Out.WriteLineAsync($"Track '{track.Title}' and associated file deleted successfully");
                return true;
            }
            else await Console.Out.WriteLineAsync($"Track with ID '{trackId}' is not found");
            return false;
        }

        #endregion
    }
}
