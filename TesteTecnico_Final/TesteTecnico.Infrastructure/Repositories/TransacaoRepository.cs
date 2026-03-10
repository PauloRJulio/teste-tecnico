using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Interfaces;
using TesteTecnico.Infrastructure.Data;

namespace TesteTecnico.Infrastructure.Repositories;

public class TransacaoRepository(IDbContextFactory<TesteTecnicoDbContext> contextFactory)
    : Repo<Transacao>(contextFactory), ITransacaoRepository
{
    public override async Task<List<Transacao>> GetAllAsync(Expression<Func<Transacao, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        if (predicate != null)
            return await context.Transacoes.Include(e => e.Pessoa).Include(e => e.Categoria).Where(predicate).ToListAsync(cancellationToken);
        
        return await context.Transacoes.Include(e => e.Pessoa).Include(e => e.Categoria).ToListAsync(cancellationToken);
    }
}