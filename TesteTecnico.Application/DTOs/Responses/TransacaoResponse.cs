using TesteTecnico.Domain.Entities;

namespace TesteTecnico.Application.DTOs.Responses;

public class TransacaoResponse
{
    public int Id { get; set; }
    public string? Descricao { get; set; }
    public decimal Valor { get; set; }
    public string TipoTransacao { get; set; }
    public string? PessoaNome { get; set; }
    public string? CategoriaDescricao { get; set; }
    public TransacaoResponse()
    {
        
    }
    public TransacaoResponse(Transacao transacao)
    {
        Id = transacao.Id;
        Descricao = transacao.Descricao;
        Valor = transacao.Valor;
        TipoTransacao = transacao.TipoTransacao.ToString();
        PessoaNome = transacao.Pessoa?.Nome;
        CategoriaDescricao = transacao.Categoria?.Descricao;
    }
}