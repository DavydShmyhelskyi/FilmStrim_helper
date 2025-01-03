﻿// <auto-generated />
using Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Core.Migrations
{
    [DbContext(typeof(FilmstripContext))]
    [Migration("20241124001424_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.AvarageRating", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<double>("avarageRating")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.ToTable("AvarageRatings");
                });

            modelBuilder.Entity("Core.Filmstrip", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("NumVotes")
                        .HasColumnType("int");

                    b.Property<int>("avarageRatingsId")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("typeId")
                        .HasColumnType("int");

                    b.Property<int>("yearReleaseId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("avarageRatingsId");

                    b.HasIndex("typeId");

                    b.HasIndex("yearReleaseId");

                    b.ToTable("Filmstrips");
                });

            modelBuilder.Entity("Core.Genre", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Core.OtherTables.FilmstripGenre", b =>
                {
                    b.Property<string>("FilmstripId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.HasKey("FilmstripId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("FilmstripGenres");
                });

            modelBuilder.Entity("Core.Types", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("Core.YearRelease", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("year")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("YearReleases");
                });

            modelBuilder.Entity("Core.Filmstrip", b =>
                {
                    b.HasOne("Core.AvarageRating", "avarageRating")
                        .WithMany("Filmstrips")
                        .HasForeignKey("avarageRatingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Types", "type")
                        .WithMany("Filmstrips")
                        .HasForeignKey("typeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.YearRelease", "yearRelease")
                        .WithMany("Filmstrips")
                        .HasForeignKey("yearReleaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("avarageRating");

                    b.Navigation("type");

                    b.Navigation("yearRelease");
                });

            modelBuilder.Entity("Core.OtherTables.FilmstripGenre", b =>
                {
                    b.HasOne("Core.Filmstrip", "Filmstrip")
                        .WithMany("FilmstripGenres")
                        .HasForeignKey("FilmstripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Genre", "Genre")
                        .WithMany("FilmstripGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Filmstrip");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Core.AvarageRating", b =>
                {
                    b.Navigation("Filmstrips");
                });

            modelBuilder.Entity("Core.Filmstrip", b =>
                {
                    b.Navigation("FilmstripGenres");
                });

            modelBuilder.Entity("Core.Genre", b =>
                {
                    b.Navigation("FilmstripGenres");
                });

            modelBuilder.Entity("Core.Types", b =>
                {
                    b.Navigation("Filmstrips");
                });

            modelBuilder.Entity("Core.YearRelease", b =>
                {
                    b.Navigation("Filmstrips");
                });
#pragma warning restore 612, 618
        }
    }
}
