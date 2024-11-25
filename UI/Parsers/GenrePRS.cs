using CsvHelper;
using Core;
using Repositorys.Context;
using System.Globalization;
using System.IO;
using Repositorys.Context;


namespace UI.Parsers
{
    public class GenreParser
    {
        public static List<Genre> Parse(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            var genres = new List<Genre>();
            while (csv.Read())
            {
                string genresField = csv.GetField("genres");

                if (!string.IsNullOrEmpty(genresField))
                {
                    var genreNames = genresField.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var genreName in genreNames)
                    {
                        var trimmedName = genreName.Trim();

                        if (!genres.Any(g => g.name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase)))
                        {
                            genres.Add(new Genre { name = trimmedName });
                        }
                    }
                }
            }
            return genres;
        }

        public static void SaveToDatabase(FilmstripContext context, List<Genre> genres)
        {
            var existingGenres = context.Genres.Select(g => g.name.ToLower()).ToHashSet();

            foreach (var genre in genres)
            {
                if (!existingGenres.Contains(genre.name.ToLower()))
                {
                    context.Genres.Add(genre);
                }
            }

            context.SaveChanges();
        }

    }
}
