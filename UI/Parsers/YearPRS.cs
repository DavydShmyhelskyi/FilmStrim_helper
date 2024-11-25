using Core;
using CsvHelper;
using System.Globalization;
using System.IO;
using Repositorys.Context;


namespace UI.Parsers
{
    class YearParser
    {
        public static List<YearRelease> Parse(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            var yearReleases = new List<YearRelease>();
            while (csv.Read())
            {
                string yearStr = csv.GetField("releaseYear");

                // Перетворення рядка у ціле число
                if (int.TryParse(yearStr, out int year))
                {
                    // Додаємо лише унікальні роки
                    if (!yearReleases.Any(y => y.year == year))
                    {
                        yearReleases.Add(new YearRelease { year = year });
                    }
                }
            }
            return yearReleases;
        }

        public static void SaveToDatabase(FilmstripContext context, List<YearRelease> yearReleases)
        {
            foreach (var yearRelease in yearReleases)
            {
                if (!context.YearReleases.Any(y => y.year == yearRelease.year))
                {
                    context.YearReleases.Add(yearRelease);
                }
            }
            context.SaveChanges();
        }
    }
}
