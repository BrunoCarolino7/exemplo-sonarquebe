using Newtonsoft.Json;

namespace api_validacao_negocio.Services.Dtos.Input;

public class EstoqueInputDto
{
    [JsonProperty("empresaEstoque")]
    public string EmpresaEstoque { get; set; } = null!;
    [JsonProperty("prodReferencia")]
    public string ProdReferencia { get; set; } = null!;
    [JsonProperty("itemPedidoId")]
    public string ItemPedidoId { get; set; } = null!;
    [JsonProperty("peso")]
    public string Peso { get; set; } = null!;
    [JsonProperty("posicao")]
    public string? Posicao { get; set; }
    [JsonProperty("negocioId")]
    public int NegocioId { get; set; }
}
