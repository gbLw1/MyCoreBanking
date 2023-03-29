using MyCoreBanking.Args;
using MyCoreBanking.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace MyCoreBanking.Web;

partial class MyCoreBankingApp
{
    public async Task<IReadOnlyCollection<ContaModel>?> ObterContas()
    {
        try
        {
            var requestUri = $"{BaseAddress}/contas";

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
                var result = JsonSerializer.Deserialize<IReadOnlyCollection<ContaModel>?>(responseContent)
                    ?? throw new InvalidOperationException(responseContent);

                return result;
            }

            ShowError(responseContent);
            return null;
        }
        catch (InvalidOperationException ex)
        {
            ShowError(ex.Message);
            return null;
        }
        catch (UnauthorizedAccessException ex)
        {
            ShowError(ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return null;
        }
    }

    public async Task<ContaModel?> ObterContaPorId(Guid contaId)
    {
        try
        {
            var requestUri = $"{BaseAddress}/contas/{contaId}";

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
                var result = JsonSerializer.Deserialize<ContaModel?>(responseContent)
                    ?? throw new InvalidOperationException(responseContent);

                return result;
            }

            ShowError(responseContent);
            return null;
        }
        catch (InvalidOperationException ex)
        {
            ShowError(ex.Message);
            return null;
        }
        catch (UnauthorizedAccessException ex)
        {
            ShowError(ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return null;
        }
    }

    public async Task CadastrarConta(ContasPostArgs args)
    {
        try
        {
            var requestUri = $"{BaseAddress}/contas";

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var jwt = await AuthorizationToken()
                ?? throw new UnauthorizedAccessException("Você deve estar logado para acessar este recurso.");

            httpRequestMessage.Headers.Add("Access-Control-Allow-Origin", "*");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt.AccessToken);

            httpRequestMessage.Content = JsonContent.Create(args);

            using var httpResponseMessage = await _HttpClientService.SendAsync(httpRequestMessage);

            if (httpResponseMessage.StatusCode is HttpStatusCode.InternalServerError)
            {
                ShowError("Sistema temporariamente indisponível");
            }

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                ShowError(responseContent);
                return;
            }
        }
        catch (InvalidOperationException ex)
        {
            ShowError(ex.Message);
            return;
        }
        catch (UnauthorizedAccessException ex)
        {
            ShowError(ex.Message);
            return;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return;
        }
    }

    public async Task AlterarConta(Guid contaId, ContasPutArgs args)
    {
        try
        {
            var requestUri = $"{BaseAddress}/contas/{contaId}";

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);

            var jwt = await AuthorizationToken()
                ?? throw new UnauthorizedAccessException("Você deve estar logado para acessar este recurso.");

            httpRequestMessage.Headers.Add("Access-Control-Allow-Origin", "*");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt.AccessToken);

            httpRequestMessage.Content = JsonContent.Create(args);

            using var httpResponseMessage = await _HttpClientService.SendAsync(httpRequestMessage);

            if (httpResponseMessage.StatusCode is HttpStatusCode.InternalServerError)
            {
                ShowError("Sistema temporariamente indisponível");
            }

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                ShowError(responseContent);
                return;
            }
        }
        catch (InvalidOperationException ex)
        {
            ShowError(ex.Message);
            return;
        }
        catch (UnauthorizedAccessException ex)
        {
            ShowError(ex.Message);
            return;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return;
        }
    }

    public async Task ExcluirConta(Guid contaId)
    {
        try
        {
            var requestUri = $"{BaseAddress}/contas/{contaId}";

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);

            var jwt = await AuthorizationToken()
                ?? throw new UnauthorizedAccessException("Você deve estar logado para acessar este recurso.");

            httpRequestMessage.Headers.Add("Access-Control-Allow-Origin", "*");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt.AccessToken);

            using var httpResponseMessage = await _HttpClientService.SendAsync(httpRequestMessage);

            if (httpResponseMessage.StatusCode is HttpStatusCode.InternalServerError)
            {
                ShowError("Sistema temporariamente indisponível");
            }

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                ShowError(responseContent);
                return;
            }
        }
        catch (InvalidOperationException ex)
        {
            ShowError(ex.Message);
            return;
        }
        catch (UnauthorizedAccessException ex)
        {
            ShowError(ex.Message);
            return;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return;
        }
    }
}