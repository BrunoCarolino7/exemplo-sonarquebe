using api_validacao_negocio.Services.Dtos.Input;
using api_validacao_negocio.Services.VerificaPrazoDoPrecoFornecedor;
using api_validacao_negocio.Settings;
using Microsoft.AspNetCore.Mvc;

namespace api_validacao_negocio.Controllers;

[ApiController]
[Route("validar")]
public class ItemController(ItemService itemService, CacheSettings cacheSettings) : ControllerBase
{
    private readonly ItemService _itemService = itemService;
    private readonly CacheSettings _cacheSettings = cacheSettings;

    [HttpPost]
    [Route("prazo-do-produto")]
    public async Task<IActionResult> ValidarPrazoDoPreco([FromQuery] ItemInputDto item)
    {
        throw new Exception("API desativada momentaneamente.");
        return Ok(await _itemService.ValidarPrazoDoPreco(item, _cacheSettings));
    }

    [HttpPost]
    [Route("condicao-especial")]
    public async Task<IActionResult> ValidarCondicaoEspecial([FromQuery] ItemInputDto item)
    {
        throw new Exception("API desativada momentaneamente.");
        return Ok(await _itemService.ValidarCondicaoEspecial(item, _cacheSettings));
    }

    [HttpPost]
    [Route("estoque")]
    public async Task<IActionResult> ValidarEstoque([FromBody] EstoqueInputDto dto, CacheSettings _cacheSettings)
    {
        return Ok(await _itemService.ValidarEstoque(dto, _cacheSettings));
    }

    [HttpPost]
    [Route("preco-pre-aprovado")]
    public async Task<IActionResult> ValidarPrecoPreAprovado([FromQuery] PrecoPreAprovadoInputDto item)
    {
        return Ok(await _itemService.ValidarForaPrecoPreAprovado(item, _cacheSettings));
    }

      [HttpPost]
    [Route("")]
    public async Task<IActionResult> TesteMethod([FromQuery] PrecoPreAprovadoInputDto item)
    {
        return Ok();
    }
}