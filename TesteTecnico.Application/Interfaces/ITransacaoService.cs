using System.Linq.Expressions;
using TesteTecnico.Application.DTOs.Inputs;
using TesteTecnico.Application.DTOs.Responses;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Application.Interfaces;

public interface ITransacaoService
{
    public Task<BaseResponse<TransacaoResponse?>> GetAsync(Expression<Func<Transacao, bool>> predicate,
        CancellationToken cancellationToken);

    public Task<List<TransacaoResponse>> GetAllAsync(string? descricao, decimal? valor, TipoTransacao? tipoTransacao, int? pessoaId, int? categoriaId, CancellationToken cancellationToken);

    public Task<BaseResponse<TransacaoResponse?>> CreateAsync(TransacaoInput input, CancellationToken cancellationToken);

    public Task<BaseResponse<TransacaoResponse?>> UpdateAsync(int id, TransacaoInput input, CancellationToken cancellationToken);

    public Task<BaseResponse<TransacaoResponse?>> DeleteAsync(int id, CancellationToken cancellationToken);
}