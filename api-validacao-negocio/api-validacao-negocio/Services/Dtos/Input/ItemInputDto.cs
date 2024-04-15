namespace api_validacao_negocio.Services.Dtos.Input;

public class ItemInputDto
{
    public int ItemId { get; set; }
    public int NegocioId { get; set; }
    public string? PedidoId { get; set; }
    public string? FatEmpresa { get; set; }
    public double? EstoqueSolicitado { get; set; }
}
