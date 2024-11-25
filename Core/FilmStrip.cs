using Core.OtherTables;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Filmstrip
    {
        [Key]
        public string id { get; set; }  // Унікальний ідентифікатор
        public string name { get; set; }  // Назва фільму
        public int NumVotes { get; set; }  // Кількість голосів

        // Зовнішні ключі
        public int typeId { get; set; }
        public int yearReleaseId { get; set; }
        public int avarageRatingsId { get; set; }

        // Навігаційні властивості
        public Types type { get; set; }
        public YearRelease yearRelease { get; set; }
        public AvarageRating avarageRating { get; set; }

        // Зв’язок "багато-до-багатьох"
        public ICollection<FilmstripGenre> FilmstripGenres { get; set; }
       
    }
}