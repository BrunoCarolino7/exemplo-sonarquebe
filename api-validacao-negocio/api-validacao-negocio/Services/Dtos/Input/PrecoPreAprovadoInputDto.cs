namespace api_validacao_negocio.Services.Dtos.Input;

public class PrecoPreAprovadoInputDto
{
    public int ItemId { get; set; }
    public int NegocioId { get; set; }
    public string? PedidoId { get; set; }
}
