using MyCoreBanking.Args;
using MyCoreBanking.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MyCoreBanking.Web;

partial class MyCoreBankingApp
{
    #region [+ Auth Session]

    ValueTask<AuthTokenModel?> AuthorizationToken()
        => _SessionStorage.GetItemAsync<AuthTokenModel?>(nameof(AuthorizationToken));

    ValueTask AuthorizationToken(AuthTokenModel token)
        => _SessionStorage.SetItemAsync(nameof(AuthorizationToken), token);

    public async Task Logout()
    {
        await _SessionStorage.RemoveItemAsync(nameof(AuthorizationToken));
        _Navigation.NavigateTo("/login");
        await Task.Delay(100);
    }

    #endregion

    public async ValueTask<bool> IsAuthorized()
        => await AuthorizationToken() is not null;

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
                var result = JsonSerializer.Deserialize<AuthTokenModel>(responseContent)
                    ?? throw new InvalidOperationException(responseContent);

                await AuthorizationToken(result);
                _Navigation.NavigateTo("");
            }

            ShowError(responseContent);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return;
        }
    }
}