using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SonicSpectrum.Application.DTOs;
using SonicSpectrum.Application.Models;
using SonicSpectrum.Application.Repository.Abstract;
using SonicSpectrum.Application.Services;
using SonicSpectrum.Domain.Entities;
using SonicSpectrum.Persistence.Data;

namespace SonicSpectrum.Application.Repository.Concrete
{
    public class MusicSettingService(AppDbContext _context) : IMusicSettingService
    {
        #region get

        public async Task<IEnumerable<object>> GetAllTracksAsync(int pageNumber, int pageSize)
        {
            var tracks = await _context.Tracks
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(t => new {
                                            t.TrackId,
                                            t.Title,
                                            t.FilePath,
                                            t.ImagePath,
                                            t.ArtistId,
                                            t.AlbumId,
                                        })
                                        .ToListAsync();

            return tracks;
        }

        #endregion


        #region Add

        public async Task<OperationResult> AddAlbumAsync(AlbumDto albumDto)
        {
            var result = new OperationResult();

            var artist = await _context.Artists.FirstOrDefaultAsync(art => art.Id == albumDto.ArtistId);

            if (artist == null)
            {
                result.Success = false;
                result.ErrorMessage = $"Artist with ID {albumDto.ArtistId} is not found.";
                return result;
            }

            if(albumDto == null || albumDto.Title == null || string.IsNullOrEmpty(albumDto.Title))
            {
                result.Success = false;
                result.ErrorMessage = $"Album is null or empty";
                return result;
            }

            try
            {
                var album = new Album { Title = albumDto.Title, ArtistId = artist.Id };
                await _context.Albums.AddAsync(album);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = $"{album} Added successfully";
                return result;
            }
            catch (DbUpdateException ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Database update error: {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"An error occurred: {ex.Message}";
                return result;
            }
        }

        public async Task<OperationResult> AddArtistAsync(ArtistDTO artistDto)
        {
            var result = new OperationResult();

            if (artistDto == null || artistDto.Name == null)
            {
                result.Success = false;
                result.ErrorMessage = "ArtistDTO or artist name is null.";
                return result;
            }

            try
            {
                var artist = new Artist { Name = artistDto.Name };
                await _context.Artists.AddAsync(artist);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = $"{artist} Added successfully";
                return result;
            }
            catch (DbUpdateException ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Database update error: {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"An error occurred: {ex.Message}";
                return result;
            }
        }

        public async Task<OperationResult> AddGenreAsync(GenreDTO genreDto)
        {
            var result = new OperationResult();

            if (genreDto == null || genreDto.Name == null || string.IsNullOrEmpty(genreDto.Name))
            {
                result.Success = false;
                result.ErrorMessage = "GenreDTO or genre name is null.";
                return result;
            }

            try
            {
                var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreDto.Name);
                if (existingGenre == null)
                {
                    var genre = new Genre { Name = genreDto.Name };
                    await _context.Genres.AddAsync(genre);
                    await _context.SaveChangesAsync();
                    result.Success = true;
                    result.Message = $"{genre} Added successfully"; 
                    return result;
                }

                result.Success = false;
                result.ErrorMessage = $"{genreDto.Name} already exists.";
                return result;
            }
            catch (DbUpdateException ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Database update error: {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"An error occurred: {ex.Message}";
                return result;
            }
        }

        public async Task<OperationResult> AddTrackAsync(TrackDTO trackDto)
        {
            var result = new OperationResult();

            if (trackDto == null || trackDto.Title == null || trackDto.ArtistId == null || trackDto.AlbumId == null
                || trackDto.FilePath == null || trackDto.ImagePath == null || string.IsNullOrEmpty(trackDto.Title))
            {
                result.Success = false;
                result.ErrorMessage = "One or more required fields are null.";
                return result;
            }

            try
            {
                var artist = await _context.Artists.FirstOrDefaultAsync(a => a.Id == trackDto.ArtistId);
                if (artist == null)
                {
                    result.Success = false;
                    result.ErrorMessage = $"Artist with ID {trackDto.ArtistId} is not found";
                    return result;
                }

                var album = await _context.Albums.FirstOrDefaultAsync(a => a.AlbumId == trackDto.AlbumId && a.ArtistId == artist.Id);
                if (album == null)
                {
                    result.Success = false;
                    result.ErrorMessage = $"Album with ID {trackDto.AlbumId} does not belong to the specified artist";
                    return result;
                }

                var track = new Track
                {
                    Title = trackDto.Title,
                    ArtistId = trackDto.ArtistId,
                    AlbumId = trackDto.AlbumId,
                    FilePath = await UploadFileHelper.UploadFile(trackDto.FilePath, "musicplay", trackDto.Title),
                    ImagePath = await UploadFileHelper.UploadFile(trackDto.ImagePath, "musicphoto", trackDto.Title)
                };

                track.Albums = new HashSet<Album>();
                track.Genres = new HashSet<Genre>();
                track.Lyrics = new HashSet<Lyric>();

                if (trackDto.AlbumTitles != null)
                {
                    foreach (var albumTitle in trackDto.AlbumTitles)
                    {
                        var albumFromTitles = await _context.Albums.FirstOrDefaultAsync(a => a.Title == albumTitle && a.ArtistId == artist.Id);
                        if (albumFromTitles != null)
                            track.Albums.Add(albumFromTitles);
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
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = $"{track} Added successfully";
                return result;
            }
            catch (DbUpdateException ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Database update error: {ex.Message}";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"An error occurred: {ex.Message}";
            }

            return result;
        }


        #endregion

        #region Edit

        public async Task<OperationResult> EditAlbumAsync(string albumId, AlbumDto albumDto)
        {
            var result = new OperationResult();

            if (string.IsNullOrEmpty(albumId))
            {
                result.Success = false;
                result.ErrorMessage = "Album ID is null or empty.";
                return result;
            }

            var existingAlbum = await _context.Albums.FindAsync(albumId);
            if (existingAlbum == null)
            {
                result.Success = false;
                result.ErrorMessage = $"Album with ID {albumId} not found.";
                return result;
            }

            if (albumDto == null || string.IsNullOrEmpty(albumDto.Title))
            {
                result.Success = false;
                result.ErrorMessage = "AlbumDTO or album title is null or empty.";
                return result;
            }

            existingAlbum.Title = albumDto.Title;

            try
            {
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = $"Album with ID {albumId} updated successfully.";
                return result;
            }
            catch (DbUpdateException ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Database update error: {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"An error occurred: {ex.Message}";
                return result;
            }
        }

        public async Task<OperationResult> EditArtistAsync(string artistId, ArtistDTO artistDto)
        {
            var result = new OperationResult();

            if (string.IsNullOrEmpty(artistId))
            {
                result.Success = false;
                result.ErrorMessage = "Artist ID is null or empty.";
                return result;
            }

            var existingArtist = await _context.Artists.FindAsync(artistId);
            if (existingArtist == null)
            {
                result.Success = false;
                result.ErrorMessage = $"Artist with ID {artistId} not found.";
                return result;
            }

            if (artistDto == null || string.IsNullOrEmpty(artistDto.Name))
            {
                result.Success = false;
                result.ErrorMessage = "ArtistDTO or artist name is null or empty.";
                return result;
            }

            existingArtist.Name = artistDto.Name; 

            try
            {
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = $"Artist with ID {artistId} updated successfully.";
                return result;
            }
            catch (DbUpdateException ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Database update error: {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"An error occurred: {ex.Message}";
                return result;
            }
        }

        public async Task<OperationResult> EditGenreAsync(string genreId, GenreDTO genreDto)
        {
            var result = new OperationResult();

            if (string.IsNullOrEmpty(genreId))
            {
                result.Success = false;
                result.ErrorMessage = "Genre ID is null or empty.";
                return result;
            }

            var existingGenre = await _context.Genres.FindAsync(genreId);
            if (existingGenre == null)
            {
                result.Success = false;
                result.ErrorMessage = $"Genre with ID {genreId} not found.";
                return result;
            }

            if (genreDto == null || string.IsNullOrEmpty(genreDto.Name))
            {
                result.Success = false;
                result.ErrorMessage = "GenreDTO or genre name is null or empty.";
                return result;
            }

            existingGenre.Name = genreDto.Name; 

            try
            {
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = $"Genre with ID {genreId} updated successfully.";
                return result;
            }
            catch (DbUpdateException ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Database update error: {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"An error occurred: {ex.Message}";
                return result;
            }
        }

        public async Task<OperationResult> EditTrackAsync(string trackId, TrackDTO trackDto)
        {
            var result = new OperationResult();

            if (string.IsNullOrEmpty(trackId))
            {
                result.Success = false;
                result.ErrorMessage = "Track ID is null or empty.";
                return result;
            }

            var existingTrack = await _context.Tracks.FindAsync(trackId);
            if (existingTrack == null)
            {
                result.Success = false;
                result.ErrorMessage = $"Track with ID {trackId} not found.";
                return result;
            }

            if (trackDto == null || string.IsNullOrEmpty(trackDto.Title) || string.IsNullOrEmpty(trackDto.ArtistId)
                || string.IsNullOrEmpty(trackDto.AlbumId) || trackDto.FilePath == null
                || trackDto.ImagePath == null)
            {
                result.Success = false;
                result.ErrorMessage = "One or more required fields are null or empty.";
                return result;
            }

            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.Id == trackDto.ArtistId);
            if (artist == null)
            {
                result.Success = false;
                result.ErrorMessage = $"Artist with ID {trackDto.ArtistId} is not found";
                return result;
            }

            var album = await _context.Albums.FirstOrDefaultAsync(a => a.AlbumId == trackDto.AlbumId && a.ArtistId == artist.Id);
            if (album == null)
            {
                result.Success = false;
                result.ErrorMessage = $"Album with ID {trackDto.AlbumId} does not belong to the specified artist";
                return result;
            }

            existingTrack.Title = trackDto.Title ?? existingTrack.Title;
            existingTrack.ArtistId = trackDto.ArtistId ?? existingTrack.ArtistId;
            existingTrack.AlbumId = trackDto.AlbumId ?? existingTrack.AlbumId;

            try
            {
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = $"Track with ID {trackId} updated successfully.";
                return result;
            }
            catch (DbUpdateException ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Database update error: {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"An error occurred: {ex.Message}";
                return result;
            }
        }



        #endregion

        #region Delete

        public async Task<OperationResult> DeleteArtistAsync(string artistId)
        {
            var result = new OperationResult();

            var artist = await _context.Artists.FindAsync(artistId);
            if (artist != null)
            {
                try
                {
                    _context.Artists.Remove(artist);
                    await _context.SaveChangesAsync();

                    result.Success = true;
                    result.Message = "Artist deleted successfully.";
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = $"{artist} is null";
            }

            return result;
        }

        public async Task<OperationResult> DeleteAlbumAsync(string albumId)
        {
            var result = new OperationResult();

            var album = await _context.Albums.FindAsync(albumId);
            if (album != null)
            {
                try
                {
                    _context.Albums.Remove(album);
                    await _context.SaveChangesAsync();

                    result.Success = true;
                    result.Message = "Album deleted successfully.";
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = $"Album with ID {albumId} is not found";
            }

            return result;
        }

        public async Task<OperationResult> DeleteGenreAsync(string genreId)
        {
            var result = new OperationResult();

            var genre = await _context.Genres.FindAsync(genreId);
            if (genre != null)
            {
                try
                {
                    _context.Genres.Remove(genre);
                    await _context.SaveChangesAsync();

                    result.Success = true;
                    result.Message = "Genre deleted successfully.";
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = $"Genre with ID {genreId} is not found";
            }

            return result;
        }

        public async Task<OperationResult> DeleteTrackAsync(string trackId)
        {
            var result = new OperationResult();

            var track = await _context.Tracks.FindAsync(trackId);
            if (track != null)
            {
                try
                {
                    _context.Tracks.Remove(track);
                    await _context.SaveChangesAsync();

                    await UploadFileHelper.DeleteFile(track.FilePath, "musicplay");
                    await UploadFileHelper.DeleteFile(track.ImagePath!, "musicphoto");

                    result.Success = true;
                    result.Message = $"Track '{track.Title}' and associated files deleted successfully";
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = $"Track with ID '{trackId}' is not found";
            }

            return result;
        }


        #endregion
    }
}
