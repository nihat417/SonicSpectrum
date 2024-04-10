using SonicSpectrum.Application.DTOs;

namespace SonicSpectrum.Application.Repository.Abstract
{
    public  interface IMusicSettingService
    {
        Task<bool> AddAlbumAsync(AlbumDto albumDto);
        Task<bool> AddArtistAsync(ArtistDTO artistDto);
        Task<bool> AddGenreAsync(GenreDTO genreDto);
        Task<bool> AddTrackAsync(TrackDTO trackDto);


        Task<bool> DeleteArtistAsync(string artistId);
        Task<bool> DeleteAlbumAsync(string albumId);
        Task<bool> DeleteGenreAsync(string genreId);
        Task<bool> DeleteTrackAsync(string trackId);

        Task<bool> GetAllMusics();
    }
}
