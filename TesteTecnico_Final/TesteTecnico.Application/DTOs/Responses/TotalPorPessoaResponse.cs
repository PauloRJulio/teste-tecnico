using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Application.DTOs.Responses;

// DTO usado para detalhar totais por pessoa no relatório
// Contém dados da pessoa e totais de receitas, despesas e saldo
public class TotalPorPessoaResponse
{
    // ID da pessoa
    public int Id { get; set; }

    // Nome da pessoa
    public string? Nome { get; set; }

    // Total de receitas associadas à pessoa
    public decimal TotalReceitas { get; set; }

    // Total de despesas associadas à pessoa
    public decimal TotalDespesas { get; set; }

    // Saldo (TotalReceitas - TotalDespesas)
    public decimal Saldo { get; set; }
    
    // Construtor que inicializa o DTO a partir de uma entidade Pessoa
    public TotalPorPessoaResponse(Pessoa pessoa)
    {
        Id = pessoa.Id;
        Nome = pessoa.Nome;

        // Calcula totais de receitas e despesas
        TotalReceitas = GetTotalReceitas(pessoa);
        TotalDespesas = GetTotalDespesas(pessoa);

        // Calcula saldo consolidado
        Saldo = TotalReceitas - TotalDespesas;
    }

    // Soma todas as transações do tipo Receita da pessoa
    private decimal GetTotalReceitas(Pessoa pessoa)
    {
        return pessoa.Transacoes
            .Where(e => e.TipoTransacao == TipoTransacao.Receita)
            .Sum(e => e.Valor);
    }
    
    // Soma todas as transações do tipo Despesa da pessoa
    private decimal GetTotalDespesas(Pessoa pessoa)
    {
        return pessoa.Transacoes
            .Where(e => e.TipoTransacao == TipoTransacao.Despesa)
            .Sum(e => e.Valor);
    }
}