using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Interfaces;
using TesteTecnico.Infrastructure.Data;

namespace TesteTecnico.Infrastructure.Repositories;

public class PessoaRepository(IDbContextFactory<TesteTecnicoDbContext> contextFactory)
    : Repo<Pessoa>(contextFactory), IPessoaRepository
{
    private readonly IDbContextFactory<TesteTecnicoDbContext> _contextFactory = contextFactory;

    public async Task<List<Pessoa>> GetAllPessoasTransacoes(Expression<Func<Pessoa, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        if (predicate != null)
            return await context.Pessoas
                .Where(predicate)
                .Include(e => e.Transacoes).ThenInclude(e => e.Categoria)
                .ToListAsync(cancellationToken);

        return await context.Pessoas
            .Include(e => e.Transacoes).ThenInclude(e => e.Categoria)
            .ToListAsync(cancellationToken);
    }
}