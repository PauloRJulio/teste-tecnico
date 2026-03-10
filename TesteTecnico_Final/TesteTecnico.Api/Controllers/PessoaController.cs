using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.DTOs.Inputs;
using TesteTecnico.Application.DTOs.Responses;
using TesteTecnico.Application.Interfaces;

namespace TesteTecnico.Controllers;

// Controller para gerenciamento de pessoas
// Define a rota base como api/Pessoa/{action}
[Route("api/[controller]/[action]")]
[ApiController]
public class PessoaController(IPessoaService pessoaService) : ControllerBase
{
    // GET api/Pessoa/Get/{id}
    // Retorna uma pessoa específica pelo ID
    // Retorno: BaseResponse com dados da pessoa
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<PessoaResponse>>> Get(int id, CancellationToken cancellationToken)
    {
        var response = await pessoaService.GetAsync(x => x.Id == id, cancellationToken);
        return Ok(response);
    }

    // GET api/Pessoa/GetAll
    // Retorna todas as pessoas cadastradas, com filtros opcionais por nome e idade
    // Retorno: lista de PessoaResponse
    [HttpGet]
    public async Task<ActionResult<List<PessoaResponse>>> GetAll([FromQuery] string? nome, [FromQuery] int? idade, CancellationToken cancellationToken)
    {
        var pessoas = await pessoaService.GetAllAsync(nome, idade, cancellationToken);
        return Ok(pessoas);
    }

    // POST api/Pessoa/Create
    // Cria uma nova pessoa a partir de um DTO de input
    // Retorno: BaseResponse com dados da pessoa criada
    [HttpPost]
    public async Task<ActionResult<BaseResponse<PessoaResponse>>> Create([FromBody] PessoaInput input, CancellationToken cancellationToken)
    {
        var response = await pessoaService.CreateAsync(input, cancellationToken);
        return Ok(response);
    }

    // DELETE api/Pessoa/Delete/{id}
    // Remove uma pessoa pelo ID
    // Retorno: BaseResponse indicando sucesso ou falha
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<PessoaResponse>>> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await pessoaService.DeleteAsync(id, cancellationToken);
        return Ok(response);
    }

    // PUT api/Pessoa/Update/{id}
    // Atualiza os dados de uma pessoa existente
    // Retorno: BaseResponse com dados atualizados
    [HttpPut("{id}")]
    public async Task<ActionResult<BaseResponse<PessoaResponse>>> Update(int id, [FromBody] PessoaInput input, CancellationToken cancellationToken)
    {
        var response = await pessoaService.UpdateAsync(id, input, cancellationToken);
        return Ok(response);
    }
    
    // GET api/Pessoa/GetRelatorioTotaisPorPessoa
    // Retorna um relatório de totais por pessoa, com filtros opcionais por nome e idade
    // Ideal para dashboards ou exportação de dados
    [HttpGet]
    public async Task<ActionResult<RelatorioResponse>> GetRelatorioTotaisPorPessoa([FromQuery] string? nome, [FromQuery] int? idade, CancellationToken cancellationToken)
    {
        var pessoas = await pessoaService.GetRelatorioTotaisPorPessoa(nome, idade, cancellationToken);
        return Ok(pessoas);
    }
}