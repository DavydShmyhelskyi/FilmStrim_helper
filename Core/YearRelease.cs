using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class YearRelease
    {
        [Key]
        public int id { get; set; }  // Унікальний ідентифікатор
        public int year { get; set; }  // Рік випуску

        // Навігаційна властивість для зв'язку "один-до-багатьох" з Filmstrip
        public ICollection<Filmstrip> Filmstrips { get; set; }
    }
}
