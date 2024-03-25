using SonicSpectrum.Application.DTOs;

namespace SonicSpectrum.Application.Repository.Abstract
{
    public  interface IMusicSettingService
    {
        Task AddArtistAsync(ArtistDTO artistDto);
        Task AddGenreAsync(GenreDTO genreDto);
        Task AddTrackAsync(TrackDTO trackDto);
        Task DeleteArtistAsync(string artistId);
        Task DeleteGenreAsync(string genreId);
        Task DeleteTrackAsync(string trackId);
    }
}
