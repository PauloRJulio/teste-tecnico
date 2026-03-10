using System.Linq.Expressions;
using TesteTecnico.Domain.Entities;

namespace TesteTecnico.Domain.Interfaces;

public interface ICategoriaRepository : IRepo<Categoria>
{
    public Task<List<Categoria>> GetAllCategoriaTransacoes(Expression<Func<Categoria, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
}