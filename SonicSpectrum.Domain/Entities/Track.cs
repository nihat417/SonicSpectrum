namespace SonicSpectrum.Domain.Entities
{
    public class Track
    {
        public string TrackId { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string? ArtistId { get; set; }
        public virtual Artist? Artist { get; set; }
        public virtual ICollection<Album> ?Albums { get; set; }
        public virtual ICollection<Genre> ?Genres { get; set; }
    }
}
