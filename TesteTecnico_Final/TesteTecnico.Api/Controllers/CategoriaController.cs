using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.DTOs.Inputs;
using TesteTecnico.Application.DTOs.Responses;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Controllers;

// Controller para gerenciamento de categorias
// Define a rota base como api/Categoria/{action}
[Route("api/[controller]/[action]")]
[ApiController]
public class CategoriaController(ICategoriaService categoriaService) : ControllerBase
{
    // GET api/Categoria/Get/{id}
    // Retorna uma categoria específica pelo ID
    // Retorno: BaseResponse com dados da categoria
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<CategoriaResponse>>> Get(int id, CancellationToken cancellationToken)
    {
        var response = await categoriaService.GetAsync(x => x.Id == id, cancellationToken);
        return Ok(response);
    }

    // GET api/Categoria/GetAll
    // Retorna todas as categorias, com filtros opcionais por descrição e finalidade
    // Retorno: lista de CategoriaResponse
    [HttpGet]
    public async Task<ActionResult<List<CategoriaResponse>>> GetAll([FromQuery] string? descricao, [FromQuery] Finalidade? finalidade, CancellationToken cancellationToken)
    {
        var categorias = await categoriaService.GetAllAsync(descricao, finalidade, cancellationToken);
        return Ok(categorias);
    }

    // POST api/Categoria/Create
    // Cria uma nova categoria a partir de um DTO de input
    // Retorno: BaseResponse com dados da categoria criada
    [HttpPost]
    public async Task<ActionResult<BaseResponse<CategoriaResponse>>> Create([FromBody] CategoriaInput input, CancellationToken cancellationToken)
    {
        var response = await categoriaService.CreateAsync(input, cancellationToken);
        return Ok(response);
    }

    // DELETE api/Categoria/Delete/{id}
    // Remove uma categoria pelo ID
    // Retorno: BaseResponse indicando sucesso ou falha
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<CategoriaResponse>>> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await categoriaService.DeleteAsync(id, cancellationToken);
        return Ok(response);
    }

    // PUT api/Categoria/Update/{id}
    // Atualiza os dados de uma categoria existente
    // Retorno: BaseResponse com dados atualizados
    [HttpPut("{id}")]
    public async Task<ActionResult<BaseResponse<CategoriaResponse>>> Update(int id, [FromBody] CategoriaInput input, CancellationToken cancellationToken)
    {
        var response = await categoriaService.UpdateAsync(id, input, cancellationToken);
        return Ok(response);
    }
    
    // GET api/Categoria/GetRelatorioTotaisPorCategoria
    // Retorna um relatório de totais por categoria, podendo filtrar por descrição e finalidade
    // Ideal para relatórios de dashboard ou exportação de dados
    [HttpGet]
    public async Task<ActionResult<RelatorioResponse>> GetRelatorioTotaisPorCategoria([FromQuery] string? descricao, [FromQuery] Finalidade? finalidade, CancellationToken cancellationToken)
    {
        var pessoas = await categoriaService.GetRelatorioTotaisPorCategoria(descricao, finalidade, cancellationToken);
        return Ok(pessoas);
    }
}