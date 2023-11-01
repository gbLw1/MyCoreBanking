using MyCoreBanking.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MyCoreBanking.Web;

partial class MyCoreBankingApp
{
    public async Task<RelatorioModel?> ObterRelatorio()
    {
        try
        {
            var requestUri = $"{BaseAddress}/relatorios";

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var jwt = await AuthorizationToken()
                ?? throw new UnauthorizedAccessException("Você deve estar logado para acessar este recurso.");

            httpRequestMessage.Headers.Add("Access-Control-Allow-Origin", "*");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt.AccessToken);

            using var httpResponseMessage = await _HttpClientService.SendAsync(httpRequestMessage);

            if (httpResponseMessage.StatusCode is HttpStatusCode.InternalServerError)
            {
                ShowError("Sistema temporariamente indisponível");
                return null;
            }

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RelatorioModel?>(responseContent)
                    ?? throw new InvalidOperationException(responseContent);

                return result;
            }

            ShowError(responseContent);
            return null;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return null;
        }
    }
}