using System.Linq.Expressions;

namespace TesteTecnico.Domain.Interfaces;

public interface IRepo<T> where T : class
{
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> InsertAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(T updated, int id, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);
}