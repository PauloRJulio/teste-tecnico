using System.Linq.Expressions;
using TesteTecnico.Application.DTOs.Inputs;
using TesteTecnico.Application.DTOs.Responses;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Utils;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Interfaces;

namespace TesteTecnico.Application.Services;

// Serviço responsável por gerenciar operações de Pessoa
// Implementa IPessoaService e utiliza IPessoaRepository para acesso ao banco
public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
{
    // Retorna uma pessoa específica pelo predicado informado
    // Se não encontrada, retorna BaseResponse com Status.NotFound
    public async Task<BaseResponse<PessoaResponse?>> GetAsync(Expression<Func<Pessoa, bool>> predicate,
        CancellationToken cancellationToken)
    {
        try
        {
            var pessoa = await pessoaRepository.GetAsync(predicate, cancellationToken);

            if (pessoa == null)
                return new BaseResponse<PessoaResponse?>(false, "Registro não localizado!", null, Status.NotFound);
            
            var pessoaResponse = new PessoaResponse(pessoa);

            return new BaseResponse<PessoaResponse?>(true, null, pessoaResponse, Status.Success);
        }
        catch (Exception e)
        {
            // Captura erros inesperados e retorna Status.Error
            return new BaseResponse<PessoaResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }

    // Retorna todas as pessoas com filtros opcionais por nome e idade
    public async Task<List<PessoaResponse>> GetAllAsync(string? nome, int? idade, CancellationToken cancellationToken)
    {
        Expression<Func<Pessoa, bool>> predicate = x => true;

        // Filtra por nome se informado
        if (!string.IsNullOrWhiteSpace(nome))
            predicate = predicate.AndAlso(x => x.Nome.Contains(nome));

        // Filtra por idade se informado
        if (idade.HasValue)
            predicate = predicate.AndAlso(x => x.Idade == idade);

        var pessoas = await pessoaRepository.GetAllAsync(predicate, cancellationToken);

        // Converte para DTO de resposta
        return pessoas.Select(x => new PessoaResponse(x)).ToList();
    }

    // Cria uma nova pessoa no banco
    public async Task<BaseResponse<PessoaResponse?>> CreateAsync(PessoaInput input, CancellationToken cancellationToken)
    {
        try
        {
            var pessoa = new Pessoa()
            {
                Nome = input.Nome,
                Idade = input.Idade,
            };
            var ret = await pessoaRepository.InsertAsync(pessoa, cancellationToken);

            if (ret == 0)
            {
                return new BaseResponse<PessoaResponse?>(false, "Falha ao criar registro!", null, Status.Error);
            }
            
            var pessoaResponse = new PessoaResponse(pessoa);

            return new BaseResponse<PessoaResponse?>(true, "Registro Cadastrado com Sucesso", pessoaResponse, Status.Success);
        }
        catch (Exception e)
        {
            return new BaseResponse<PessoaResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }

    // Atualiza uma pessoa existente pelo ID
    public async Task<BaseResponse<PessoaResponse?>> UpdateAsync(int id, PessoaInput input, CancellationToken cancellationToken)
    {
        try
        {
            var pessoa = await pessoaRepository.GetAsync(x => x.Id == id, cancellationToken);

            if (pessoa == null)
                return new BaseResponse<PessoaResponse?>(false, "Registro a ser alterado não pode ser localizado!", null, Status.NotFound);
            
            pessoa.Nome = input.Nome;
            pessoa.Idade = input.Idade;

            pessoa.SetUpdated(); // Marca como atualizado (atualiza timestamp, audit, etc.)

            var ret = await pessoaRepository.UpdateAsync(pessoa, pessoa.Id, cancellationToken);

            if (ret == 0)
            {
                return new BaseResponse<PessoaResponse?>(false, "Falha ao editar registro!", null, Status.Error);
            }
            
            var pessoaResponse = new PessoaResponse(pessoa);

            return new BaseResponse<PessoaResponse?>(true, "Registro Alterado com Sucesso", pessoaResponse, Status.Success);
        }
        catch (Exception e)
        {
            return new BaseResponse<PessoaResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }

    // Remove uma pessoa existente pelo ID
    public async Task<BaseResponse<PessoaResponse?>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var pessoa = await pessoaRepository.GetAsync(x => x.Id == id, cancellationToken);
            
            if (pessoa == null)
                return new BaseResponse<PessoaResponse?>(false, "Registro a ser excluido não pode ser localizado!", null, Status.NotFound);
            
            var ret = await pessoaRepository.DeleteAsync(pessoa, cancellationToken);
            
            if (ret == 0)
            {
                return new BaseResponse<PessoaResponse?>(false, "Falha ao excluir registro!", null, Status.Error);
            }
            
            var pessoaResponse = new PessoaResponse(pessoa);
            return new BaseResponse<PessoaResponse?>(true, "Registro excluido com Sucesso", pessoaResponse, Status.Success);
        }
        catch (Exception e)
        {
            return new BaseResponse<PessoaResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }
    
    // Gera relatório de totais por pessoa
    public async Task<RelatorioResponse> GetRelatorioTotaisPorPessoa(string? nome, int? idade, CancellationToken cancellationToken)
    {
        Expression<Func<Pessoa, bool>> predicate = x => true;

        // Filtra por nome se informado
        if (!string.IsNullOrWhiteSpace(nome))
            predicate = predicate.AndAlso(x => x.Nome.Contains(nome));

        // Filtra por idade se informado
        if (idade.HasValue)
            predicate = predicate.AndAlso(x => x.Idade == idade);

        var pessoas = await pessoaRepository.GetAllPessoasTransacoes(predicate, cancellationToken);

        // Converte cada pessoa em TotalPorPessoaResponse e gera o RelatorioResponse
        var totalPessoas = pessoas.Select(x => new TotalPorPessoaResponse(x)).ToList();
        
        return new RelatorioResponse(totalPessoas);
    }
}