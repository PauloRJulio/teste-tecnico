using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Interfaces;
using TesteTecnico.Infrastructure.Data;

namespace TesteTecnico.Infrastructure.Repositories;

public class CategoriaRepository(IDbContextFactory<TesteTecnicoDbContext> contextFactory)
    : Repo<Categoria>(contextFactory), ICategoriaRepository
{
    private readonly IDbContextFactory<TesteTecnicoDbContext> _contextFactory = contextFactory;

    public async Task<List<Categoria>> GetAllCategoriaTransacoes(Expression<Func<Categoria, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        if (predicate != null)
            return await context.Categorias
                .Where(predicate)
                .Include(e => e.Transacoes)
                .ToListAsync(cancellationToken);

        return await context.Categorias
            .Include(e => e.Transacoes)
            .ToListAsync(cancellationToken);
    }
}