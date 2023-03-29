using MyCoreBanking.Args;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyCoreBanking.Web;

partial class MyCoreBankingApp
{

    #region [+ Auth Session]

    public class AuthResponse
    {
        [JsonPropertyName("tokenType")]
        public string? TokenType { get; set; }

        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("expiresIn")]
        public string? ExpiresIn { get; set; }
    }

    ValueTask<AuthResponse?> AuthorizationToken()
        => _SessionStorage.GetItemAsync<AuthResponse?>(nameof(AuthorizationToken));

    ValueTask AuthorizationToken(AuthResponse token)
        => _SessionStorage.SetItemAsync(nameof(AuthorizationToken), token);

    public async Task Logout()
    {
        await _SessionStorage.RemoveItemAsync(nameof(AuthorizationToken));
        _Navigation.NavigateTo("/login");
        await Task.Delay(100);
    }

    #endregion

    public async ValueTask<bool> IsAuthorized() => await AuthorizationToken() is not null;


    public async Task Login(AuthTokenPostArgs args)
    {
        try
        {
            var requestUri = $"{BaseAddress}/auth/token";

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            httpRequestMessage.Headers.Add("Access-Control-Allow-Origin", "*");

            httpRequestMessage.Content = JsonContent.Create(args);

            using var httpResponseMessage = await _HttpClientService.SendAsync(httpRequestMessage);

            if (httpResponseMessage.StatusCode is HttpStatusCode.InternalServerError)
            {
                ShowError("Sistema temporariamente indisponível");
                return;
            }

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<AuthResponse>(responseContent)
                    ?? throw new InvalidOperationException(responseContent);

                await AuthorizationToken(result);
                _Navigation.NavigateTo("");
            }

            ShowError(responseContent);
        }
        catch (InvalidOperationException ex)
        {
            // Exibir modal de erro com mensagem da API
            ShowError(ex.Message);
            return;
        }
        catch (Exception ex)
        {
            // Exibir modal de erro com mensagem da API
            ShowError(ex.Message);
            return;
        }
    }
}