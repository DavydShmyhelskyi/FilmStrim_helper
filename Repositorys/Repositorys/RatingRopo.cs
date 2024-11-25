using Core;
using Repositorys.Context;
using Repositorys.Interfaces;

namespace Repositorys.Repositories
{
    public class AvarageRatingRepository : BaseInterface<AvarageRating>
    {
        private readonly FilmstripContext _context;

        public AvarageRatingRepository(FilmstripContext context)
        {
            _context = context;
        }

        public void Create(AvarageRating entity)
        {
            _context.AvarageRatings.Add(entity);
        }

        public IEnumerable<AvarageRating> Get()
        {
            return _context.AvarageRatings.ToList();
        }

        public AvarageRating Get(int id)
        {
            return _context.AvarageRatings.Find(id);
        }

        public void Update(AvarageRating entity)
        {
            _context.AvarageRatings.Update(entity);
        }

        public void Delete(int id)
        {
            var avarageRating = Get(id);
            if (avarageRating != null)
                _context.AvarageRatings.Remove(avarageRating);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
