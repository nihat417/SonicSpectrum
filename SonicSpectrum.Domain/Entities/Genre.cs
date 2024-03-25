namespace SonicSpectrum.Domain.Entities
{
    public class Genre
    {
        public string GenreId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
