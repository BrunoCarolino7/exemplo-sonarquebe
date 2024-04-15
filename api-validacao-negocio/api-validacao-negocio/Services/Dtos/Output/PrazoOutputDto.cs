using System.Text.Json.Serialization;

namespace api_validacao_negocio.Services.Dtos.Output;

public class PrazoOutputDto
{
    [JsonPropertyName("dataCadastro")]
    public string? DataCadastroString { get; set; }

    [JsonPropertyName("prazoPreco")]
    public int? PrazoPreco { get; set; }

    public string? DescricaoProduto { get; set; }

    public DateTime DataCadastro
    {
        get
        {
            if (DateTime.TryParseExact(DataCadastroString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime data))
            {
                return data;
            }

            throw new Exception("Erro ao converter data.");
        }
    }
    public DateTime? DataPrazo { get { return DataCadastro!.AddDays(PrazoPreco!.Value); } }
}
