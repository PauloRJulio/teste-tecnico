using System.Linq.Expressions;
using TesteTecnico.Application.DTOs.Inputs;
using TesteTecnico.Application.DTOs.Responses;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Utils;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Enums;
using TesteTecnico.Domain.Interfaces;

namespace TesteTecnico.Application.Services;

// Serviço responsável por gerenciar operações de Transações financeiras
// Implementa ITransacaoService e utiliza repositórios de Transacao, Pessoa e Categoria
public class TransacaoService(ITransacaoRepository transacaoRepository, IPessoaRepository pessoaRepository, ICategoriaRepository categoriaRepository) : ITransacaoService
{
    // Retorna uma transação específica pelo predicado informado
    // Se não encontrada, retorna BaseResponse com Status.NotFound
    public async Task<BaseResponse<TransacaoResponse?>> GetAsync(Expression<Func<Transacao, bool>> predicate,
        CancellationToken cancellationToken)
    {
        try
        {
            var transacao = await transacaoRepository.GetAsync(predicate, cancellationToken);

            if (transacao == null)
                return new BaseResponse<TransacaoResponse?>(false, "Registro não localizado!", null, Status.NotFound);
            
            var transacaoResponse = new TransacaoResponse(transacao);

            return new BaseResponse<TransacaoResponse?>(true, null, transacaoResponse, Status.Success);
        }
        catch (Exception e)
        {
            return new BaseResponse<TransacaoResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }

    // Retorna todas as transações com filtros opcionais: descricao, valor, tipoTransacao, pessoaId e categoriaId
    public async Task<List<TransacaoResponse>> GetAllAsync(string? descricao, decimal? valor, TipoTransacao? tipoTransacao, int? pessoaId, int? categoriaId , CancellationToken cancellationToken)
    {
        Expression<Func<Transacao, bool>> predicate = x => true;

        if (!string.IsNullOrWhiteSpace(descricao))
            predicate = predicate.AndAlso(x => x.Descricao.Contains(descricao));

        if (tipoTransacao.HasValue)
            predicate = predicate.AndAlso(x => x.TipoTransacao == tipoTransacao);
        
        if (valor.HasValue)
            predicate = predicate.AndAlso(x => x.Valor == valor.Value);
        
        if (pessoaId.HasValue)
            predicate = predicate.AndAlso(x => x.PessoaId == pessoaId);
        
        if (categoriaId.HasValue)
            predicate = predicate.AndAlso(x => x.CategoriaId == categoriaId);

        var transacaos = await transacaoRepository.GetAllAsync(predicate, cancellationToken);

        return transacaos.Select(x => new TransacaoResponse(x)).ToList();
    }

    // Cria uma nova transação
    public async Task<BaseResponse<TransacaoResponse?>> CreateAsync(TransacaoInput input, CancellationToken cancellationToken)
    {
        try
        {
            // Verifica se a pessoa existe
            var pessoaEntity = await pessoaRepository.GetAsync(e => e.Id == input.PessoaId, cancellationToken);
            if (pessoaEntity == null)
                return new BaseResponse<TransacaoResponse?>(false, "Não foi localizado Pessoa da transação.", null, Status.NotFound);

            // Valida idade: menores de 18 só podem registrar despesas
            if (pessoaEntity.Idade < 18 && input.TipoTransacao != TipoTransacao.Despesa)
                return new BaseResponse<TransacaoResponse?>(false, "Pessoa da transação menor de 18 anos, permitido apenas lançar despesas.", null, Status.Warning);
            
            // Verifica se a categoria existe
            var categoryEntity = await categoriaRepository.GetAsync(e => e.Id == input.CategoriaId, cancellationToken);
            if (categoryEntity == null)
                return new BaseResponse<TransacaoResponse?>(false, "Não foi localizado categoria selecionada para essa transação.", null, Status.NotFound);

            // Valida compatibilidade tipo transação X finalidade da categoria
            if (input.TipoTransacao.ToString() != categoryEntity.Finalidade.ToString() && categoryEntity.Finalidade != Finalidade.Ambos)
                return new BaseResponse<TransacaoResponse?>(false, "Tipo da transação incompativel com a categoria selecionada", null, Status.Warning);

            // Criação da transação
            var transacao = new Transacao()
            {
                Descricao = input.Descricao,
                Valor = input.Valor,
                TipoTransacao = input.TipoTransacao,
                PessoaId = input.PessoaId,
                CategoriaId = input.CategoriaId,
            };
            var ret = await transacaoRepository.InsertAsync(transacao, cancellationToken);

            if (ret == 0)
                return new BaseResponse<TransacaoResponse?>(false, "Falha ao criar registro!", null, Status.Error);
            
            var transacaoResponse = new TransacaoResponse(transacao);

            return new BaseResponse<TransacaoResponse?>(true, "Registro Cadastrado com Sucesso", transacaoResponse, Status.Success);
        }
        catch (Exception e)
        {
            return new BaseResponse<TransacaoResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }

    // Atualiza uma transação existente pelo ID
    public async Task<BaseResponse<TransacaoResponse?>> UpdateAsync(int id, TransacaoInput input, CancellationToken cancellationToken)
    {
        try
        {
            var transacao = await transacaoRepository.GetAsync(x => x.Id == id, cancellationToken);

            if (transacao == null)
                return new BaseResponse<TransacaoResponse?>(false, "Registro a ser alterado não pode ser localizado!", null, Status.NotFound);

            // Atualiza campos
            transacao.Descricao = input.Descricao;
            transacao.Valor = input.Valor;
            transacao.TipoTransacao = input.TipoTransacao;
            transacao.PessoaId = input.PessoaId;
            transacao.CategoriaId = input.CategoriaId;

            transacao.SetUpdated(); // Atualiza timestamps/audit

            var ret = await transacaoRepository.UpdateAsync(transacao, transacao.Id, cancellationToken);

            if (ret == 0)
                return new BaseResponse<TransacaoResponse?>(false, "Falha ao editar registro!", null, Status.Error);
            
            var transacaoResponse = new TransacaoResponse(transacao);

            return new BaseResponse<TransacaoResponse?>(true, "Registro Alterado com Sucesso", transacaoResponse, Status.Success);
        }
        catch (Exception e)
        {
            return new BaseResponse<TransacaoResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }

    // Remove uma transação existente pelo ID
    public async Task<BaseResponse<TransacaoResponse?>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var transacao = await transacaoRepository.GetAsync(x => x.Id == id, cancellationToken);
            
            if (transacao == null)
                return new BaseResponse<TransacaoResponse?>(false, "Registro a ser excluido não pode ser localizado!", null, Status.NotFound);
            
            var ret = await transacaoRepository.DeleteAsync(transacao, cancellationToken);
            
            if (ret == 0)
                return new BaseResponse<TransacaoResponse?>(false, "Falha ao excluir registro!", null, Status.Error);
            
            var transacaoResponse = new TransacaoResponse(transacao);
            return new BaseResponse<TransacaoResponse?>(true, "Registro excluido com Sucesso", transacaoResponse, Status.Success);
        }
        catch (Exception e)
        {
            return new BaseResponse<TransacaoResponse?>(false, $"Erro: {e.Message}", null, Status.Error);
        }
    }
}