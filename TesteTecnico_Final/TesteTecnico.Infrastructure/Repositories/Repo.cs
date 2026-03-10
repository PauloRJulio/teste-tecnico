using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TesteTecnico.Domain.Interfaces;
using TesteTecnico.Infrastructure.Data;

namespace TesteTecnico.Infrastructure.Repositories;

public class Repo<T>(IDbContextFactory<TesteTecnicoDbContext> contextFactory) : IRepo<T> where T : class
{
    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Set<T>().AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

            if (predicate != null)
                return await context.Set<T>().Where(predicate).ToListAsync(cancellationToken);
            return await context.Set<T>().ToListAsync(cancellationToken);
    }

    public virtual async Task<int> InsertAsync(T entity, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        await context.Set<T>().AddAsync(entity, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> UpdateAsync(T updated, int id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var entity = await context.Set<T>().FindAsync([id], cancellationToken);
        if (entity == null) return 0;

        context.Entry(entity).CurrentValues.SetValues(updated);
        context.Entry(entity).State = EntityState.Modified;
        return await context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        context.Set<T>().Remove(entity);
        return await context.SaveChangesAsync(cancellationToken);
    }
}