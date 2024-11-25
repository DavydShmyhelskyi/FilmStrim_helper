using Core;
using Repositorys.Context;
using Repositorys.Interfaces;

namespace Repositorys.Repositories
{
    public class GenreRepository : BaseInterface<Genre>
    {
        private readonly FilmstripContext _context;

        public GenreRepository(FilmstripContext context)
        {
            _context = context;
        }

        public void Create(Genre entity)
        {
            _context.Genres.Add(entity);
        }

        public IEnumerable<Genre> Get()
        {
            return _context.Genres.ToList();
        }

        public Genre Get(int id)
        {
            return _context.Genres.Find(id);
        }

        public void Update(Genre entity)
        {
            _context.Genres.Update(entity);
        }

        public void Delete(int id)
        {
            var genre = Get(id);
            if (genre != null)
                _context.Genres.Remove(genre);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
