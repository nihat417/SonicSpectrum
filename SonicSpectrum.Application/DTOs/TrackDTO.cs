namespace SonicSpectrum.Application.DTOs
{
    public class TrackDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string FilePath { get; set; } 
        public IEnumerable<string> AlbumTitles { get; set; } 
        public IEnumerable<string> GenreNames { get; set; } 

    }
}
