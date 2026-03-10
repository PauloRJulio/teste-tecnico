using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Application.DTOs.Responses;

// DTO usado para detalhar totais por categoria no relatório
// Contém dados da categoria e totais de receitas, despesas e saldo
public class TotalPorCategoriaResponse
{
    // ID da categoria
    public int Id { get; set; }

    // Descrição da categoria
    public string? Descricao { get; set; }
    
    // Finalidade da categoria (ex.: Receita ou Despesa)
    public string? Finalidade { get; set; }
    
    // Total de receitas associadas à categoria
    public decimal TotalReceitas { get; set; }

    // Total de despesas associadas à categoria
    public decimal TotalDespesas { get; set; }

    // Saldo (TotalReceitas - TotalDespesas)
    public decimal Saldo { get; set; }

    // Construtor que inicializa o DTO a partir de uma entidade Categoria
    public TotalPorCategoriaResponse(Categoria categoria)
    {
        Id = categoria.Id;
        Descricao = categoria.Descricao;
        Finalidade = categoria.Finalidade.ToString();

        // Calcula totais de receitas e despesas
        TotalReceitas = GetTotalReceitas(categoria);
        TotalDespesas = GetTotalDespesas(categoria);

        // Calcula saldo consolidado
        Saldo = TotalReceitas - TotalDespesas;
    }

    // Soma todas as transações do tipo Receita da categoria
    private decimal GetTotalReceitas(Categoria categoria)
    {
        return categoria.Transacoes
                        .Where(e => e.TipoTransacao == TipoTransacao.Receita)
                        .Sum(e => e.Valor);
    }

    // Soma todas as transações do tipo Despesa da categoria
    private decimal GetTotalDespesas(Categoria categoria)
    {
        return categoria.Transacoes
                        .Where(e => e.TipoTransacao == TipoTransacao.Despesa)
                        .Sum(e => e.Valor);
    }
}