namespace TesteTecnico.Domain.Entities;

public class Pessoa : Default
{
    public string? Nome { get; set; }
    public int Idade { get; set; }
    
    public List<Transacao> Transacoes { get; set; } = [];
}