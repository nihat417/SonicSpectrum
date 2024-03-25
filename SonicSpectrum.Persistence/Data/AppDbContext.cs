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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Album>()
                .HasOne(a => a.Artist)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Track>()
                .HasOne(t => t.Artist)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Album>()
                .HasOne(a => a.Artist)
                .WithMany(a => a.Albums)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Track>()
                .HasOne(t => t.Artist)
                .WithMany(a => a.Tracks)
                .HasForeignKey(t => t.ArtistId); 

            builder.Entity<Artist>()
                .HasMany(a => a.Tracks)
                .WithOne(t => t.Artist)
                .HasForeignKey(t => t.ArtistId);
        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
