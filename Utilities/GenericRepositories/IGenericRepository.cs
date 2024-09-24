using System.Linq.Expressions;

namespace Utilities.GenericRepositories
{
    public interface IGenericRepository<T>
    {
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
