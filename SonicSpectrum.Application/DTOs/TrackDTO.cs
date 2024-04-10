using Microsoft.AspNetCore.Http;

namespace SonicSpectrum.Application.DTOs
{
    public class TrackDTO
    {
        public string? Title { get; set; }
        public IFormFile? FilePath { get; set; } 
        public string? ArtistId { get; set; }
        public IEnumerable<string>? AlbumTitles { get; set; } 
        public IEnumerable<string>? GenreNames { get; set; } 

    }
}
