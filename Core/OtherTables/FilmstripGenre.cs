namespace Core.OtherTables
{
    public class FilmstripGenre
    {
        // Composite Key
        public string FilmstripId { get; set; } // FK до Filmstrip
        public int GenreId { get; set; }        // FK до Genre

        // Навігаційні властивості
        public Filmstrip Filmstrip { get; set; }
        public Genre Genre { get; set; }
    }
}
