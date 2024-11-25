using Core;
using Repositorys.Context;
using Repositorys.Interfaces;

namespace Repositorys.Repositories
{
    public class TypesRepository : BaseInterface<Types>
    {
        private readonly FilmstripContext _context;

        public TypesRepository(FilmstripContext context)
        {
            _context = context;
        }

        public void Create(Types entity)
        {
            _context.Types.Add(entity);
        }

        public IEnumerable<Types> Get()
        {
            return _context.Types.ToList();
        }

        public Types Get(int id)
        {
            return _context.Types.Find(id);
        }

        public void Update(Types entity)
        {
            _context.Types.Update(entity);
        }

        public void Delete(int id)
        {
            var type = Get(id);
            if (type != null)
                _context.Types.Remove(type);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
