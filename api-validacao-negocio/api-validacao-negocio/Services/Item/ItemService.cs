using api_validacao_negocio.Services.Dtos.Input;
using api_validacao_negocio.Services.Dtos.Output;
using api_validacao_negocio.Settings;
using Newtonsoft.Json;
using ServiceQueue.TadeuPorNegocioNotification;
using System.Globalization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace api_validacao_negocio.Services.VerificaPrazoDoPrecoFornecedor;

public class ItemService
{
    readonly CultureInfo cultura = new("en-US");

    public async Task<bool> ValidarPrazoDoPreco(ItemInputDto itemInputDto, CacheSettings _cacheSettings)
    {
        PrazoOutputDto prazoOutputDto = await ObterPrazoDoPreco(itemInputDto, _cacheSettings);

        DateTime hoje = DateTime.Now;

        if (prazoOutputDto.DataPrazo >= hoje)
            return true;

        TadeuPorNegocio tadeu = new()
        {
            Mensagem = $"O preço do item {prazoOutputDto.DescricaoProduto} está fora do prazo.",
            NegocioId = itemInputDto.NegocioId
        };

        tadeu.Send();

        return false;
    }
    public async Task<bool> ValidarCondicaoEspecial(ItemInputDto itemInputDto, CacheSettings _cacheSettings)
    {
        CondicaoEspecialOutputDto condicaoEspecialOutputDto = await ObterCondicaoEspecial(itemInputDto, _cacheSettings);

        if (condicaoEspecialOutputDto.CondicaoEspecial!.Trim() == "" || condicaoEspecialOutputDto.CondicaoEspecial == "0" || condicaoEspecialOutputDto.CondicaoEspecial == null || int.Parse(condicaoEspecialOutputDto.CondicaoEspecial) == 0)
            return true;

        TadeuPorNegocio tadeu = new()
        {
            Mensagem = $"Desconto aplicado incompatível.",
            NegocioId = itemInputDto.NegocioId
        };

        tadeu.Send();

        return false;
    }
    public async Task<bool> ValidarEstoque(EstoqueInputDto dto, CacheSettings _cacheSettings)
    {
        var response = await HttpResponse.ResponseContentEstoque(dto, _cacheSettings);

        try
        {
            var objectResponse = JsonConvert.DeserializeObject<EstoqueOutputDto>(response)!;

            if (objectResponse.ItemSemEstoque == 0)
                return true;

            TadeuPorNegocio tadeu = new()
            {
                Mensagem = $"Estoque insuficiente, a quantidade disponível é de {(objectResponse.EstoqueSaldo!).ToString("0.00", cultura)}kg.",
                NegocioId = dto.NegocioId,
                OrganismoUri = "estoqueFalta",
                ParametrosOrganismo = $"{{\"itemId\": {dto.ProdReferencia}}}, {{\"quantidadeDisponivel\": {(objectResponse.EstoqueSaldo!).ToString("0.00", cultura)}}}"
            };

            tadeu.Send();

            return false;
        }
        catch (Exception e)
        {
            throw new Exception($"Erro ao fazer a requisição: {e.Message}");
        }
    }
    public async Task<bool> ValidarForaPrecoPreAprovado(PrecoPreAprovadoInputDto precoPreAprovadoInputDto, CacheSettings _cacheSettings)
    {
        var response = await HttpResponse.ResponseContentPrecoPreAprovado(precoPreAprovadoInputDto, _cacheSettings);

        try
        {
            var objectResponse = JsonSerializer.Deserialize<PrecoPreAprovadoOutputDto>(response);
            var (estaNoLimite, percentualDeReducao) = objectResponse!.EstaNoLimitePreAprovado();

            if (estaNoLimite)
                return true;

            TadeuPorNegocio tadeu = new()
            {
                Mensagem = $"O item {precoPreAprovadoInputDto.ItemId} está com o preço abaixo do valor pré aprovado que é de no minimo R$ {objectResponse.PrecoMinimoPreAprovado.ToString("0.00", cultura)}/kg. O valor solicitado é de R$ {objectResponse.PrecoDisponivel.ToString("0.00", cultura)}, {percentualDeReducao.ToString("0.00", cultura):F2}% abaixo do valor minimo pré aprovado. O que você gostaria de fazer?",
                NegocioId = precoPreAprovadoInputDto.NegocioId,
                OrganismoUri = "precoPreAprovado",
                ParametrosOrganismo = $"{{\"itemId\": {precoPreAprovadoInputDto.ItemId}}}"
            };

            tadeu.Send();

            return false;
        }
        catch (Exception e)
        {
            throw new Exception($"Erro ao fazer a requisição: {e.Message}");
        }
    }
    private async Task<PrazoOutputDto> ObterPrazoDoPreco(ItemInputDto itemInputDto, CacheSettings _cacheSettings)
    {
        try
        {
            using var client = new HttpClient();

            string uri = $"{_cacheSettings.UriPrazoDoPreco}{itemInputDto.ItemId}";

            var response = await client.GetAsync(uri);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao obter prazo do item.");

            return JsonSerializer.Deserialize<PrazoOutputDto>(responseContent)!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter prazo do item. {ex.Message}");
        }
    }
    private async Task<CondicaoEspecialOutputDto> ObterCondicaoEspecial(ItemInputDto itemInputDto, CacheSettings _cacheSettings)
    {
        try
        {
            using var client = new HttpClient();

            string uri = $"{_cacheSettings.UriCondicaoEspecial}{itemInputDto.ItemId}/{itemInputDto.PedidoId}";

            var response = await client.GetAsync(uri);

            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao obter condição especial do item.");


            return JsonSerializer.Deserialize<CondicaoEspecialOutputDto>(responseContent)!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao obter condição especial do item. {ex.Message}");
        }
    }
}