using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SonicSpectrum.Domain.Entities;
using System.Reflection.Emit;

namespace SonicSpectrum.Persistence.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Albums)
                .WithOne(album => album.Artist)
                .HasForeignKey(album => album.ArtistId);

            modelBuilder.Entity<Album>()
                .HasMany(album => album.Tracks)
                .WithOne()
                .HasForeignKey(track => track.AlbumId);

            modelBuilder.Entity<Album>()
                .HasMany(album => album.Tracks)
                .WithOne(track => track.Album)
                .HasForeignKey(track => track.AlbumId);


            modelBuilder.Entity<Track>()
                .HasMany(track => track.Genres)
                .WithMany(genre => genre.Tracks)
                .UsingEntity(j => j.ToTable("TrackGenres"));

            modelBuilder.Entity<Track>()
                .HasMany(track => track.Lyrics)
                .WithOne(lyric => lyric.Track)
                .HasForeignKey(lyric => lyric.TrackId);

            modelBuilder.Entity<Track>()
                .HasMany(track => track.Playlists)
                .WithMany(playlist => playlist.Tracks)
                .UsingEntity(j => j.ToTable("TrackPlaylists"));

        }



        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Lyric> Lyrics { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
    }
}
