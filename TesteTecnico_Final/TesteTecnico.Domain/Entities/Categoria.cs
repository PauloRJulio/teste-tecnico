using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Domain.Entities;

public class Categoria : Default
{
    public string? Descricao { get; set; }
    public Finalidade Finalidade { get; set; }

    public List<Transacao> Transacoes { get; set; } = [];
}