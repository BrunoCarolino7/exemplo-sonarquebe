using api_validacao_negocio.Services.Dtos.Input;
using api_validacao_negocio.Settings;
using System.Text;

namespace api_validacao_negocio;

public static class HttpResponse
{
    public static async Task<string> ResponseContentEstoque(EstoqueInputDto dto, CacheSettings _cacheSettings)
    {
        try
        {
            using var client = new HttpClient();
            string uri = $"{_cacheSettings.UriEstoque}";

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao obter estoque do item. StatusCode: {response.StatusCode}");

            return responseContent;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter estoque do item. {ex.Message}");
        }
    }

    public static async Task<string> ResponseContentPrecoPreAprovado(PrecoPreAprovadoInputDto precoPreAprovadoInputDto, CacheSettings _cacheSettings)
    {
        try
        {
            using var client = new HttpClient();

            string uri = $"{_cacheSettings.UriForaPrecoPreAprovado}{precoPreAprovadoInputDto.ItemId}/{precoPreAprovadoInputDto.PedidoId}";

            var response = await client.GetAsync(uri);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao obter preço pré aprovado do item.");
            }

            return responseContent;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter preço pré aprovado do item. {ex.Message}");
        }
    }
}
