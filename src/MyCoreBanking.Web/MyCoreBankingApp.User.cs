using MyCoreBanking.Args;
using MyCoreBanking.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace MyCoreBanking.Web;

partial class MyCoreBankingApp
{
    public async Task CadastrarUsuario(UsuariosPostArgs args)
    {
        try
        {
            var requestUri = $"{BaseAddress}/usuarios";

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

            _Navigation.NavigateTo("/login", true);
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

    public async Task<UsuarioModel?> ObterPerfil()
    {
        try
        {
            var requestUri = $"{BaseAddress}/usuarios/perfil";

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
                var result = JsonSerializer.Deserialize<UsuarioModel>(responseContent)
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

    public async Task AtualizarPerfil(UsuariosPutArgs args)
    {
        try
        {
            var requestUri = $"{BaseAddress}/usuarios";

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

            _Navigation.NavigateTo("", true);
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