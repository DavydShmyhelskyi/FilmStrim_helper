using CsvHelper;
using Core;
using Repositorys.Context;
using System.Globalization;
using System.IO;
using Core.OtherTables;
using Repositorys.Context;


namespace UI.Parsers
{
    public class FilmstripParser
    {
        public static List<Filmstrip> Parse(string filePath, FilmstripContext context)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            var filmstrips = new List<Filmstrip>();
            while (csv.Read())
            {
                string id = csv.GetField("id");
                string name = csv.GetField("title");
                int numVotes = int.Parse(csv.GetField("numVotes"));

                string releaseYearStr = csv.GetField("releaseYear");
                int yearReleaseId = GetYearReleaseId(releaseYearStr, context);

                string avgRatingStr = csv.GetField("averageRating");
                int avarageRatingsId = GetAverageRatingId(avgRatingStr, context);

                var filmstrip = new Filmstrip
                {
                    id = id,
                    name = name,
                    NumVotes = numVotes,
                    yearReleaseId = yearReleaseId,
                    avarageRatingsId = avarageRatingsId,
                };

                // Обробка жанрів
                filmstrip.FilmstripGenres = ParseGenres(csv.GetField("genres"), context, filmstrip);

                // Тип фільму
                filmstrip.typeId = GetTypeId(csv.GetField("type"), context);

                filmstrips.Add(filmstrip);
            }
            return filmstrips;
        }

        public static void SaveToDatabase(FilmstripContext context, List<Filmstrip> filmstrips)
        {
            foreach (var filmstrip in filmstrips)
            {
                if (!context.Filmstrips.Any(f => f.id == filmstrip.id))
                {
                    context.Filmstrips.Add(filmstrip);
                }
            }
            context.SaveChanges();
        }

        private static int GetYearReleaseId(string yearStr, FilmstripContext context)
        {
            if (int.TryParse(yearStr, out int releaseYear))
            {
                var yearRelease = context.YearReleases.FirstOrDefault(y => y.year == releaseYear)
                    ?? new YearRelease { year = releaseYear };
                if (yearRelease.id == 0) context.YearReleases.Add(yearRelease);
                context.SaveChanges();
                return yearRelease.id;
            }
            throw new Exception($"Invalid year format: {yearStr}");
        }


        private static int GetAverageRatingId(string avgRatingStr, FilmstripContext context)
        {
            if (double.TryParse(avgRatingStr.Replace(".", ","), out var avgRating))
            {
                var rating = context.AvarageRatings.FirstOrDefault(a => a.avarageRating == avgRating)
                    ?? new AvarageRating { avarageRating = avgRating };
                if (rating.id == 0) context.AvarageRatings.Add(rating);
                context.SaveChanges();
                return rating.id;
            }
            throw new Exception($"Invalid average rating format: {avgRatingStr}");
        }

        private static ICollection<FilmstripGenre> ParseGenres(string genresStr, FilmstripContext context, Filmstrip filmstrip)
        {
            var genreNames = genresStr.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(g => g.Trim());
            var filmstripGenres = new List<FilmstripGenre>();

            foreach (var genreName in genreNames)
            {
                var genre = context.Genres.FirstOrDefault(g => g.name == genreName)
                    ?? new Genre { name = genreName };
                if (genre.id == 0)
                {
                    context.Genres.Add(genre);
                    context.SaveChanges();
                }
                filmstripGenres.Add(new FilmstripGenre { Filmstrip = filmstrip, Genre = genre });
            }

            return filmstripGenres;
        }

        private static int GetTypeId(string typeStr, FilmstripContext context)
        {
            var type = context.Types.FirstOrDefault(t => t.name == typeStr)
                ?? new Types { name = typeStr };
            if (type.id == 0)
            {
                context.Types.Add(type);
                context.SaveChanges();
            }
            return type.id;
        }
    }
}
