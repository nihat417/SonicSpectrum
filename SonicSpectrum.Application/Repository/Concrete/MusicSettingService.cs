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
        public async Task AddArtistAsync(ArtistDTO artistDto)
        {
            var artist = new Artist {Name = artistDto.Name!};
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();
        }

        public async Task AddGenreAsync(GenreDTO genreDto)
        {
            var genre = new Genre { Name = genreDto.Name! };
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
        }

        public async Task AddTrackAsync(TrackDTO trackDto)
        {
            var artist = await _context.Artists.FindAsync(trackDto.ArtistId);
            if (artist == null)
            {
                await Console.Out.WriteLineAsync($"{artist} is not found");
                return;
            }
            var track = new Track
            {
                Title = trackDto.Title!,
                ArtistId = trackDto.ArtistId!,
                FilePath = await UploadFileHelper.UploadFile(trackDto.FilePath!),
            };

            if (trackDto.AlbumTitles != null)
            {
                foreach (var albumTitle in trackDto.AlbumTitles)
                {
                    var album = await _context.Albums.FirstOrDefaultAsync(a => a.Title == albumTitle);
                    if (album != null) track.Albums!.Add(album);
                }
            }

            if (trackDto.GenreNames != null)
            {
                foreach (var genreName in trackDto.GenreNames)
                {
                    var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                    if (genre != null) track.Genres!.Add(genre);
                }
            }
            await _context.Tracks.AddAsync(track);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArtistAsync(string artistId)
        {
            var artist =  await _context.Artists.FindAsync(artistId);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
            }
            await Console.Out.WriteLineAsync($"{artist} is null");
        }

        public async Task DeleteGenreAsync(string genreId)
        {
            var genre = await _context.Genres.FindAsync(genreId);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
            await Console.Out.WriteLineAsync($"{genre} is null");
        }

        public async Task DeleteTrackAsync(string trackId)
        {
            var track = await _context.Tracks.FindAsync(trackId);
            if(track != null)
            {
                _context.Tracks.Remove(track);
                await _context.SaveChangesAsync();
            }
            await Console.Out.WriteLineAsync($"{track} is not found");
        }
    }
}
