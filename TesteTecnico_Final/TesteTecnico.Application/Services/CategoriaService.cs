using System.Linq.Expressions;
using TesteTecnico.Application.DTOs.Inputs;
using TesteTecnico.Application.DTOs.Responses;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Utils;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Enums;
using TesteTecnico.Domain.Interfaces;

namespace TesteTecnico.Application.Services;

// Serviço responsável por gerenciar operações de Categoria
// Implementa ICategoriaService e utiliza ICategoriaRepository para acesso ao banco
public class CategoriaService(ICategoriaRepository categoriaRepository) : ICategoriaService
{
    // Retorna uma categoria específica pelo predicado informado
    // Se não encontrada, retorna BaseResponse com Status.NotFound
    public async Task<BaseResponse<CategoriaResponse?>> GetAsync(Expression<Func<Categoria, bool>> predicate,
        CancellationToken cancellationToken)
    {
        try
        {
            var categoria = await categoriaRepository.GetAsync(predicate, cancellationToken);

            if (categoria == null)
                return new BaseResponse<CategoriaResponse?>(false, "Registro não localizado!", null, Status.NotFound);
            
            var categoriaResponse = new CategoriaResponse(categoria);

            return new BaseResponse<CategoriaResponse?>(true, null, categoriaResponse, Status.Success);
        }
        catch (Exception e)
        {
            // Captura erros inesperados e retorna Status.Error
            return new BaseResponse<CategoriaResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }

    // Retorna todas as categorias com filtros opcionais por descricao e finalidade
    public async Task<List<CategoriaResponse>> GetAllAsync(string? descricao, Finalidade? finalidade, CancellationToken cancellationToken)
    {
        Expression<Func<Categoria, bool>> predicate = x => true;

        // Filtra por descricao se informado
        if (!string.IsNullOrWhiteSpace(descricao))
            predicate = predicate.AndAlso(x => x.Descricao.Contains(descricao));

        // Filtra por finalidade se informado
        if (finalidade.HasValue)
            predicate = predicate.AndAlso(x => x.Finalidade == finalidade);

        var categorias = await categoriaRepository.GetAllAsync(predicate, cancellationToken);

        // Converte para DTO de resposta
        return categorias.Select(x => new CategoriaResponse(x)).ToList();
    }

    // Cria uma nova categoria no banco
    public async Task<BaseResponse<CategoriaResponse?>> CreateAsync(CategoriaInput input, CancellationToken cancellationToken)
    {
        try
        {
            var categoria = new Categoria()
            {
                Descricao = input.Descricao,
                Finalidade = input.Finalidade,
            };
            var ret = await categoriaRepository.InsertAsync(categoria, cancellationToken);

            if (ret == 0)
            {
                return new BaseResponse<CategoriaResponse?>(false, "Falha ao criar registro!", null, Status.Error);
            }
            
            var categoriaResponse = new CategoriaResponse(categoria);

            return new BaseResponse<CategoriaResponse?>(true, "Registro Cadastrado com Sucesso", categoriaResponse, Status.Success);
        }
        catch (Exception e)
        {
            return new BaseResponse<CategoriaResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }

    // Atualiza uma categoria existente pelo ID
    public async Task<BaseResponse<CategoriaResponse?>> UpdateAsync(int id, CategoriaInput input, CancellationToken cancellationToken)
    {
        try
        {
            var categoria = await categoriaRepository.GetAsync(x => x.Id == id, cancellationToken);

            if (categoria == null)
                return new BaseResponse<CategoriaResponse?>(false, "Registro a ser alterado não pode ser localizado!", null, Status.NotFound);
            
            categoria.Descricao = input.Descricao;
            categoria.Finalidade = input.Finalidade;

            categoria.SetUpdated(); // Marca como atualizado (atualiza timestamp, audit, etc.)

            var ret = await categoriaRepository.UpdateAsync(categoria, categoria.Id, cancellationToken);

            if (ret == 0)
            {
                return new BaseResponse<CategoriaResponse?>(false, "Falha ao editar registro!", null, Status.Error);
            }
            
            var categoriaResponse = new CategoriaResponse(categoria);

            return new BaseResponse<CategoriaResponse?>(true, "Registro Alterado com Sucesso", categoriaResponse, Status.Success);
        }
        catch (Exception e)
        {
            return new BaseResponse<CategoriaResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }

    // Remove uma categoria existente pelo ID
    public async Task<BaseResponse<CategoriaResponse?>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var categoria = await categoriaRepository.GetAsync(x => x.Id == id, cancellationToken);
            
            if (categoria == null)
                return new BaseResponse<CategoriaResponse?>(false, "Registro a ser excluido não pode ser localizado!", null, Status.NotFound);
            
            var ret = await categoriaRepository.DeleteAsync(categoria, cancellationToken);
            
            if (ret == 0)
            {
                return new BaseResponse<CategoriaResponse?>(false, "Falha ao excluir registro!", null, Status.Error);
            }
            
            var categoriaResponse = new CategoriaResponse(categoria);

            return new BaseResponse<CategoriaResponse?>(true, "Registro excluido com Sucesso", categoriaResponse, Status.Success);
        }
        catch (Exception e)
        {
            return new BaseResponse<CategoriaResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }
    
    // Gera relatório de totais por categoria
    public async Task<RelatorioResponse> GetRelatorioTotaisPorCategoria(string? descricao, Finalidade? finalidade, CancellationToken cancellationToken)
    {
        Expression<Func<Categoria, bool>> predicate = x => true;

        // Filtra por descricao se informado
        if (!string.IsNullOrWhiteSpace(descricao))
            predicate = predicate.AndAlso(x => x.Descricao.Contains(descricao));

        // Filtra por finalidade se informado
        if (finalidade.HasValue)
            predicate = predicate.AndAlso(x => x.Finalidade == finalidade);

        var categorias = await categoriaRepository.GetAllCategoriaTransacoes(predicate, cancellationToken);

        // Converte cada categoria em TotalPorCategoriaResponse e gera o RelatorioResponse
        var totalCategorias = categorias.Select(x => new TotalPorCategoriaResponse(x)).ToList();
        
        return new RelatorioResponse(totalCategorias);
    }
}