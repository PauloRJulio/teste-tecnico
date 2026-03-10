using System.ComponentModel.DataAnnotations;
using TesteTecnico.Domain.Entities;

namespace TesteTecnico.Application.DTOs.Inputs;

// DTO usado para criar ou atualizar uma pessoa
// Validações importantes:
// Nome: obrigatório, máximo de 200 caracteres
// Idade: deve ser maior ou igual a zero e menor ou igual a 150
public class PessoaInput
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(200, ErrorMessage = "Nome não pode ter mais de 200 caracteres")]
    public string? Nome { get; set; }

    [Range(0, 150, ErrorMessage = "Idade deve ser maior ou igual a zero")]
    public int Idade { get; set; }

    public PessoaInput()
    {
        
    }
    
    // Construtor que inicializa o DTO a partir de uma entidade Pessoa
    public PessoaInput(Pessoa pessoa)
    {
        Nome = pessoa.Nome;
        Idade = pessoa.Idade;
    }
}