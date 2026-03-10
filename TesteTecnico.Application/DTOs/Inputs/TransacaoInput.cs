using System.ComponentModel.DataAnnotations;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Application.DTOs.Inputs;

// DTO usado para criar ou atualizar uma transação
// Validações importantes:
// Descricao: obrigatória, máximo de 400 caracteres
// Valor: deve ser maior que zero
public class TransacaoInput
{
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [MaxLength(400, ErrorMessage = "Descrição não pode ter mais de 400 caracteres")]
    public string? Descricao { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Valor { get; set; }

    public TipoTransacao TipoTransacao { get; set; }

    public int PessoaId { get; set; }

    public int CategoriaId { get; set; }

    public TransacaoInput()
    {
    }

    // Construtor que inicializa o DTO a partir de uma entidade Transacao
    public TransacaoInput(Transacao transacao)
    {
        Descricao = transacao.Descricao;
        Valor = transacao.Valor;
        TipoTransacao = transacao.TipoTransacao;
        PessoaId = transacao.PessoaId;
        CategoriaId = transacao.CategoriaId;
    }
}