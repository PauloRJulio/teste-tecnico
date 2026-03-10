using TesteTecnico.Domain.Entities;

namespace TesteTecnico.Application.DTOs.Responses;

public class PessoaResponse
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public int Idade { get; set; }
    public PessoaResponse()
    {
        
    }
    public PessoaResponse(Pessoa pessoa)
    {
        Id = pessoa.Id;
        Nome = pessoa.Nome;
        Idade = pessoa.Idade;
    }
}