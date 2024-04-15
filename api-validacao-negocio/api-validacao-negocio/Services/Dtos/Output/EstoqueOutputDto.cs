namespace api_validacao_negocio.Services.Dtos.Output;

public class EstoqueOutputDto
{
    public int EstoqueDisponivel { get; set; }
    public int EstoqueNaoConforme { get; set; }
    public int EstoqueReserva { get; set; }
    public double EstoqueSaldo { get; set; }
    public int ItemSemEstoque { get; set; }
}