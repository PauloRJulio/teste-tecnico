using System.Linq.Expressions;
using TesteTecnico.Application.DTOs.Inputs;
using TesteTecnico.Application.DTOs.Responses;
using TesteTecnico.Domain.Entities;

namespace TesteTecnico.Application.Interfaces;

public interface IPessoaService
{
    public Task<BaseResponse<PessoaResponse?>> GetAsync(Expression<Func<Pessoa, bool>> predicate,
        CancellationToken cancellationToken);

    public Task<List<PessoaResponse>> GetAllAsync(string? nome, int? idade, CancellationToken cancellationToken);

    public Task<BaseResponse<PessoaResponse?>> CreateAsync(PessoaInput input, CancellationToken cancellationToken);

    public Task<BaseResponse<PessoaResponse?>> UpdateAsync(int id, PessoaInput input, CancellationToken cancellationToken);

    public Task<BaseResponse<PessoaResponse?>> DeleteAsync(int id, CancellationToken cancellationToken);

    public Task<RelatorioResponse> GetRelatorioTotaisPorPessoa(string? nome, int? idade,
        CancellationToken cancellationToken);
}