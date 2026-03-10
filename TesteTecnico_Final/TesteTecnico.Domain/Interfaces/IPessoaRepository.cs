using System.Linq.Expressions;
using TesteTecnico.Domain.Entities;

namespace TesteTecnico.Domain.Interfaces;

public interface IPessoaRepository : IRepo<Pessoa>
{
    public Task<List<Pessoa>> GetAllPessoasTransacoes(Expression<Func<Pessoa, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
}