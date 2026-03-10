using System.Linq.Expressions;
using TesteTecnico.Application.DTOs.Inputs;
using TesteTecnico.Application.DTOs.Responses;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Application.Interfaces;

public interface ICategoriaService
{
    public Task<BaseResponse<CategoriaResponse?>> GetAsync(Expression<Func<Categoria, bool>> predicate,
        CancellationToken cancellationToken);

    public Task<List<CategoriaResponse>> GetAllAsync(string? descricao, Finalidade? finalidade, CancellationToken cancellationToken);

    public Task<BaseResponse<CategoriaResponse?>> CreateAsync(CategoriaInput input, CancellationToken cancellationToken);

    public Task<BaseResponse<CategoriaResponse?>> UpdateAsync(int id, CategoriaInput input, CancellationToken cancellationToken);

    public Task<BaseResponse<CategoriaResponse?>> DeleteAsync(int id, CancellationToken cancellationToken);

    public Task<RelatorioResponse> GetRelatorioTotaisPorCategoria(string? descricao, Finalidade? finalidade,
        CancellationToken cancellationToken);
}