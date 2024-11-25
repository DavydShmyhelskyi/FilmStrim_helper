using Core;
using Repositorys.Context;
using Repositorys.Interfaces;

namespace Repositorys.Repositories
{
    public class YearReleaseRepository : BaseInterface<YearRelease>
    {
        private readonly FilmstripContext _context;

        public YearReleaseRepository(FilmstripContext context)
        {
            _context = context;
        }

        public void Create(YearRelease entity)
        {
            _context.YearReleases.Add(entity);
        }

        public IEnumerable<YearRelease> Get()
        {
            return _context.YearReleases.ToList();
        }

        public YearRelease Get(int id)
        {
            return _context.YearReleases.Find(id);
        }

        public void Update(YearRelease entity)
        {
            _context.YearReleases.Update(entity);
        }

        public void Delete(int id)
        {
            var yearRelease = Get(id);
            if (yearRelease != null)
                _context.YearReleases.Remove(yearRelease);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
