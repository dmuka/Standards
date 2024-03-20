using System.Linq.Expressions;

namespace Standards.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        Task<List<T>> GetAllAsync();

        T GetById(int id);

        T GetByIdWithIncludes(int id);

        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdWithIncludesAsync(int id);

        bool Remove(int id);

        void Add(in T entity);

        void Update(in T entity);

        int Save();

        Task<int> SaveAsync();

        public T Select(Expression<Func<T, bool>> predicate);

        public Task<T> SelectAsync(Expression<Func<T, bool>> predicate);
    }
}
