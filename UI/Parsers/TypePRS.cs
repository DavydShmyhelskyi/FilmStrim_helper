using CsvHelper;
using Core;
using Repositorys.Context;
using System.Globalization;
using System.IO;
using Repositorys.Context;


namespace UI.Parsers
{
    public class TypeParser
    {
        public static List<Types> Parse(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            var types = new List<Types>();
            while (csv.Read())
            {
                var typeField = csv.GetField("type");

                if (!string.IsNullOrEmpty(typeField))
                {
                    var trimmedName = typeField.Trim();

                    if (!types.Any(t => t.name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase)))
                    {
                        types.Add(new Types { name = trimmedName });
                    }
                }
            }
            return types;
        }

        public static void SaveToDatabase(FilmstripContext context, List<Types> types)
        {
            var existingTypes = context.Types.Select(t => t.name.ToLower()).ToHashSet();

            foreach (var type in types)
            {
                if (!existingTypes.Contains(type.name.ToLower()))
                {
                    context.Types.Add(type);
                }
            }

            context.SaveChanges();
        }

    }
}
