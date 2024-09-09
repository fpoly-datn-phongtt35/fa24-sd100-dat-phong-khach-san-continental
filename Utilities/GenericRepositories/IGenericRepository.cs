using System.Linq.Expressions;

namespace Utilities.GenericRepositories
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> AllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T?> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
