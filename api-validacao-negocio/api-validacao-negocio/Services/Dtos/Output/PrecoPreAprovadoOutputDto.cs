using System.Text.Json.Serialization;

namespace api_validacao_negocio.Services.Dtos.Output;

public class PrecoPreAprovadoOutputDto
{
    [JsonPropertyName("precoTela")]
    public string? PrecoDisponivelString { get; set; }

    [JsonPropertyName("precoMinimoPreAprovado")]
    public string? PrecoMinimoPreAprovadoString { get; set; }

    public double PrecoDisponivel { get => double.Parse(PrecoDisponivelString!); }
    public double PrecoMinimoPreAprovado { get => double.Parse(PrecoMinimoPreAprovadoString!); }
    public string Descricao { get; set; } = null!;

    public (bool, double) EstaNoLimitePreAprovado()
    {
        double diferenca = Math.Abs(PrecoMinimoPreAprovado - PrecoDisponivel);
        double percentualDeReducao = (diferenca / PrecoMinimoPreAprovado) * 100;

        return (PrecoDisponivel < PrecoMinimoPreAprovado, percentualDeReducao);
    }
}
