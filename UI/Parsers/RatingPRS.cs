using CsvHelper;
using Core;
using Repositorys.Context;
using System.Globalization;
using System.IO;
using Repositorys.Context;


namespace UI.Parsers
{
    public class RatingParser
    {
        public static List<AvarageRating> Parse(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            var avgRatings = new List<AvarageRating>();
            while (csv.Read())
            {
                string ratingStr = csv.GetField("averageRating").Replace(".", ",");

                if (double.TryParse(ratingStr, out double rating))
                {
                    if (!avgRatings.Any(a => a.avarageRating == rating))
                    {
                        avgRatings.Add(new AvarageRating { avarageRating = rating });
                    }
                }
            }
            return avgRatings;
        }

        public static void SaveToDatabase(FilmstripContext context, List<AvarageRating> avgRatings)
        {
            foreach (var rating in avgRatings)
            {
                if (!context.AvarageRatings.Any(a => a.avarageRating == rating.avarageRating))
                {
                    context.AvarageRatings.Add(rating);
                }
            }
            context.SaveChanges();
        }
    }
}
