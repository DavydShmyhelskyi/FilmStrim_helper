using Microsoft.EntityFrameworkCore;
using Core;
using Core.OtherTables;

namespace Repositorys.Context
{
    public class FilmstripContext : DbContext
    {
        public DbSet<Filmstrip> Filmstrips => Set<Filmstrip>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Types> Types => Set<Types>();
        public DbSet<YearRelease> YearReleases => Set<YearRelease>();
        public DbSet<AvarageRating> AvarageRatings => Set<AvarageRating>();
        public DbSet<FilmstripGenre> FilmstripGenres => Set<FilmstripGenre>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433; Database=MoviesDBNoWayToLive; User=sa; Password=Admin123!Braaah1; Encrypt=True; TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Налаштування зв’язку "багато-до-багатьох" між Filmstrip та Genre через FilmstripGenre
            modelBuilder.Entity<FilmstripGenre>()
                .HasKey(fg => new { fg.FilmstripId, fg.GenreId }); // Композитний ключ

            modelBuilder.Entity<FilmstripGenre>()
                .HasOne(fg => fg.Filmstrip)
                .WithMany(f => f.FilmstripGenres)
                .HasForeignKey(fg => fg.FilmstripId);

            modelBuilder.Entity<FilmstripGenre>()
                .HasOne(fg => fg.Genre)
                .WithMany(g => g.FilmstripGenres)
                .HasForeignKey(fg => fg.GenreId);

            // Зв’язок "один-до-багатьох" між Filmstrip та YearRelease
            modelBuilder.Entity<Filmstrip>()
                .HasOne(f => f.yearRelease)
                .WithMany(yr => yr.Filmstrips)
                .HasForeignKey(f => f.yearReleaseId)
                .OnDelete(DeleteBehavior.Cascade); // Опціонально: каскадне видалення

            // Зв’язок "один-до-багатьох" між Filmstrip та AvarageRating
            modelBuilder.Entity<Filmstrip>()
                .HasOne(f => f.avarageRating)
                .WithMany(ar => ar.Filmstrips)
                .HasForeignKey(f => f.avarageRatingsId)
                .OnDelete(DeleteBehavior.Cascade); // Опціонально: каскадне видалення

            // Зв’язок "один-до-багатьох" між Filmstrip та Types
            modelBuilder.Entity<Filmstrip>()
                .HasOne(f => f.type)
                .WithMany(t => t.Filmstrips)
                .HasForeignKey(f => f.typeId)
                .OnDelete(DeleteBehavior.Cascade); // Опціонально: каскадне видалення

            base.OnModelCreating(modelBuilder);
        }
    }
}
