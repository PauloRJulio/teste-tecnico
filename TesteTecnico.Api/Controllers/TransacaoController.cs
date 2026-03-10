using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.DTOs.Inputs;
using TesteTecnico.Application.DTOs.Responses;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Controllers;

// Controller para gerenciamento de transações
// Define a rota base como api/Transacao/{action}
[Route("api/[controller]/[action]")]
[ApiController]
public class TransacaoController(ITransacaoService transacaoService) : ControllerBase
{
    // GET api/Transacao/Get/{id}
    // Retorna uma transação específica pelo ID
    // Retorno: BaseResponse com dados da transação
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<TransacaoResponse>>> Get(int id, CancellationToken cancellationToken)
    {
        var response = await transacaoService.GetAsync(x => x.Id == id, cancellationToken);
        return Ok(response);
    }

    // GET api/Transacao/GetAll
    // Retorna todas as transações, com filtros opcionais por:
    // descricao, valor, tipoTransacao, pessoaId e categoriaId
    // Retorno: lista de TransacaoResponse
    [HttpGet]
    public async Task<ActionResult<List<TransacaoResponse>>> GetAll(
        [FromQuery] string? descricao,
        [FromQuery] decimal? valor,
        [FromQuery] TipoTransacao? tipoTransacao,
        [FromQuery] int? pessoaId,
        [FromQuery] int? categoriaId,
        CancellationToken cancellationToken)
    {
        var transacaos = await transacaoService.GetAllAsync(descricao, valor, tipoTransacao, pessoaId, categoriaId, cancellationToken);
        return Ok(transacaos);
    }

    // POST api/Transacao/Create
    // Cria uma nova transação a partir de um DTO de input
    // Retorno: BaseResponse com dados da transação criada
    [HttpPost]
    public async Task<ActionResult<BaseResponse<TransacaoResponse>>> Create([FromBody] TransacaoInput input, CancellationToken cancellationToken)
    {
        var response = await transacaoService.CreateAsync(input, cancellationToken);
        return Ok(response);
    }

    // DELETE api/Transacao/Delete/{id}
    // Remove uma transação pelo ID
    // Retorno: BaseResponse indicando sucesso ou falha
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<TransacaoResponse>>> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await transacaoService.DeleteAsync(id, cancellationToken);
        return Ok(response);
    }

    // PUT api/Transacao/Update/{id}
    // Atualiza os dados de uma transação existente
    // Retorno: BaseResponse com dados atualizados
    [HttpPut("{id}")]
    public async Task<ActionResult<BaseResponse<TransacaoResponse>>> Update(int id, [FromBody] TransacaoInput input, CancellationToken cancellationToken)
    {
        var response = await transacaoService.UpdateAsync(id, input, cancellationToken);
        return Ok(response);
    }
}