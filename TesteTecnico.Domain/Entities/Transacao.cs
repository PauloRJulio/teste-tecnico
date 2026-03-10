using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Domain.Entities;

public class Transacao : Default
{
    public string? Descricao { get; set; }
    public decimal Valor { get; set; }
    public TipoTransacao TipoTransacao { get; set; }
    public int PessoaId { get; set; }
    public int CategoriaId { get; set; }
    public Pessoa Pessoa { get; set; }
    public Categoria Categoria { get; set; }
}