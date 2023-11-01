using Blazored.Modal.Services;
using Blazored.SessionStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace MyCoreBanking.Web;

partial class MyCoreBankingApp
{
    private readonly IModalService _ModalService;
    private readonly IToastService _ToastService;
    private readonly ISessionStorageService _SessionStorage;
    private readonly NavigationManager _Navigation;
    private readonly HttpClient _HttpClientService;

    private readonly string BaseAddress;

    public MyCoreBankingApp(
            IModalService modalService,
            IToastService toastService,
            ISessionStorageService sessionStorage,
            NavigationManager navigation,
            HttpClient httpClientService)
    {
        _ModalService = modalService;
        _ToastService = toastService;
        _SessionStorage = sessionStorage;
        _Navigation = navigation;
        _HttpClientService = httpClientService;

        if (_Navigation.Uri.Contains("localhost"))
        {
            BaseAddress = "http://localhost:7071";
        }
        else
        {
            BaseAddress = "https://api-mycore-finance.azurewebsites.net";
        }
    }

    public async Task Initialize()
    {
        if (!await IsAuthorized())
        {
            _Navigation.NavigateTo("login");
            throw new UnauthorizedAccessException();
        }
    }
}