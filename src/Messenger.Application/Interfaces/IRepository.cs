namespace Messenger.Application.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void SaveChanges();
        Task AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }
}
