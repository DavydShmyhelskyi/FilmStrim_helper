namespace Repositorys.Interfaces
{
    public interface BaseInterfaceFILMS<T>
    {
        void Create(T entity);
        IEnumerable<T> Get();
        T Get(string id);
        void Update(T entity);
        void Delete(string id);
        void SaveChanges();
    }
}
