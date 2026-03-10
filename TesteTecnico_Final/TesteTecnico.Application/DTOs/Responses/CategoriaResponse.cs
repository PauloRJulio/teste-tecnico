using TesteTecnico.Domain.Entities;

namespace TesteTecnico.Application.DTOs.Responses;

public class CategoriaResponse
{
    public int Id { get; set; }
    public string? Descricao { get; set; }
    
    public string? Finalidade { get; set; }
    public CategoriaResponse()
    {
        
    }
    public CategoriaResponse(Categoria categoria)
    {
        Id = categoria.Id;
        Descricao = categoria.Descricao;
        Finalidade = categoria.Finalidade.ToString();
    }
}