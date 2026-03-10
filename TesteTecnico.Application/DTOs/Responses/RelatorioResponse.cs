namespace TesteTecnico.Application.DTOs.Responses;

// DTO usado para retornar relatórios consolidados de transações
// Contém totais gerais e listas detalhadas por pessoa ou categoria
public class RelatorioResponse
{
    // Total de receitas somadas
    public decimal TotalReceitas { get; set; }

    // Total de despesas somadas
    public decimal TotalDespesas { get; set; }

    // Saldo consolidado (receitas - despesas)
    public decimal SaldoConsolidado { get; set; }

    // Lista detalhada de totais por pessoa (opcional)
    public List<TotalPorPessoaResponse>? TotaisPorPessoas { get; set; }
    
    // Lista detalhada de totais por categoria (opcional)
    public List<TotalPorCategoriaResponse>? TotaisPorCategorias { get; set; }
    
    // Construtor que gera relatório a partir de lista de totais por pessoa
    public RelatorioResponse(List<TotalPorPessoaResponse> relatorioTotaisPorPessoas)
    {
        // Soma total de receitas, despesas e saldo
        TotalReceitas = relatorioTotaisPorPessoas.Sum(e => e.TotalReceitas);
        TotalDespesas = relatorioTotaisPorPessoas.Sum(e => e.TotalDespesas);
        SaldoConsolidado = relatorioTotaisPorPessoas.Sum(e => e.Saldo);

        // Armazena a lista detalhada
        TotaisPorPessoas = relatorioTotaisPorPessoas;
    }
    
    // Construtor que gera relatório a partir de lista de totais por categoria
    public RelatorioResponse(List<TotalPorCategoriaResponse> relatorioTotaisPorCategorias)
    {
        // Soma total de receitas, despesas e saldo
        TotalReceitas = relatorioTotaisPorCategorias.Sum(e => e.TotalReceitas);
        TotalDespesas = relatorioTotaisPorCategorias.Sum(e => e.TotalDespesas);
        SaldoConsolidado = relatorioTotaisPorCategorias.Sum(e => e.Saldo);

        // Armazena a lista detalhada
        TotaisPorCategorias = relatorioTotaisPorCategorias;
    }
}