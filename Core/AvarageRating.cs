using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class AvarageRating
    {
        [Key]
        public int id { get; set; }  // Унікальний ідентифікатор
        public double avarageRating { get; set; }  // Середній рейтинг

        // Навігаційна властивість для зв'язку "один-до-багатьох" з Filmstrip
        public ICollection<Filmstrip> Filmstrips { get; set; }
    }
}
