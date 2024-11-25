using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvarageRatings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    avarageRating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvarageRatings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "YearReleases",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearReleases", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Filmstrips",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumVotes = table.Column<int>(type: "int", nullable: false),
                    typeId = table.Column<int>(type: "int", nullable: false),
                    yearReleaseId = table.Column<int>(type: "int", nullable: false),
                    avarageRatingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filmstrips", x => x.id);
                    table.ForeignKey(
                        name: "FK_Filmstrips_AvarageRatings_avarageRatingsId",
                        column: x => x.avarageRatingsId,
                        principalTable: "AvarageRatings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Filmstrips_Types_typeId",
                        column: x => x.typeId,
                        principalTable: "Types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Filmstrips_YearReleases_yearReleaseId",
                        column: x => x.yearReleaseId,
                        principalTable: "YearReleases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmstripGenres",
                columns: table => new
                {
                    FilmstripId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmstripGenres", x => new { x.FilmstripId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_FilmstripGenres_Filmstrips_FilmstripId",
                        column: x => x.FilmstripId,
                        principalTable: "Filmstrips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmstripGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmstripGenres_GenreId",
                table: "FilmstripGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Filmstrips_avarageRatingsId",
                table: "Filmstrips",
                column: "avarageRatingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Filmstrips_typeId",
                table: "Filmstrips",
                column: "typeId");

            migrationBuilder.CreateIndex(
                name: "IX_Filmstrips_yearReleaseId",
                table: "Filmstrips",
                column: "yearReleaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmstripGenres");

            migrationBuilder.DropTable(
                name: "Filmstrips");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "AvarageRatings");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "YearReleases");
        }
    }
}
