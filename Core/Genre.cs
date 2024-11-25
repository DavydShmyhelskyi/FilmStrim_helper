using Core.OtherTables;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Genre
    {
        [Key]
        public int id { get; set; }  // Унікальний ідентифікатор
        public string name { get; set; }  // Назва жанру

        // Зв’язок "багато-до-багатьох"
        public ICollection<FilmstripGenre> FilmstripGenres { get; set; }
    }
}
