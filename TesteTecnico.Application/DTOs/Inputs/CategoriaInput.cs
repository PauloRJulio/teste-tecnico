using System.ComponentModel.DataAnnotations;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Application.DTOs.Inputs;

// DTO usado para criar ou atualizar uma categoria
// Validações importantes:
// Descricao: obrigatória, máximo de 400 caracteres
// Finalidade: enum que indica o tipo da categoria 
public class CategoriaInput
{
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [MaxLength(400, ErrorMessage = "Descrição não pode ter mais de 400 caracteres")]
    public string? Descricao { get; set; }
    
    public Finalidade Finalidade { get; set; }

    public CategoriaInput()
    {
        
    }

    // Construtor que inicializa o DTO a partir de uma entidade Categoria
    public CategoriaInput(Categoria categoria)
    {
        Descricao = categoria.Descricao;
        Finalidade = categoria.Finalidade;
    }
}