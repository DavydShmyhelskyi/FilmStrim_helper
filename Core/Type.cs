using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Types
    {
        [Key]
        public int id { get; set; }  // Унікальний ідентифікатор
        public string name { get; set; }  // Назва типу (наприклад, "tvSeries")

        // Навігаційна властивість для зв'язку "один-до-багатьох" з Filmstrip
        public ICollection<Filmstrip> Filmstrips { get; set; }
    }

}
