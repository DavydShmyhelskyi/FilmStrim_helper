using Core;
using Repositorys.Context;
using Repositorys.Interfaces;

namespace Repositorys.Repositories
{
    public class FilmstripRepository : BaseInterfaceFILMS<Filmstrip>
    {
        private readonly FilmstripContext _context;

        public FilmstripRepository(FilmstripContext context)
        {
            _context = context;
        }

        public void Create(Filmstrip entity)
        {
            _context.Filmstrips.Add(entity);
        }

        public IEnumerable<Filmstrip> Get()
        {
            return _context.Filmstrips.ToList();
        }

        public Filmstrip Get(string id)
        {
            return _context.Filmstrips.Find(id);
        }

        public void Update(Filmstrip entity)
        {
            _context.Filmstrips.Update(entity);
        }

        public void Delete(string id)
        {
            var filmstrip = Get(id);
            if (filmstrip != null)
                _context.Filmstrips.Remove(filmstrip);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
