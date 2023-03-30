using Blazored.Modal.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;

namespace MyCoreBanking.Web;

partial class MyCoreBankingApp
{
    private readonly IModalService _ModalService;
    private readonly NavigationManager _Navigation;
    private readonly ISessionStorageService _SessionStorage;
    private readonly HttpClient _HttpClientService;

    private readonly string BaseAddress;

    public MyCoreBankingApp(
            IModalService modalService,
            NavigationManager navigation,
            ISessionStorageService sessionStorage,
            HttpClient httpClientService)
    {
        _ModalService = modalService;
        _Navigation = navigation;
        _SessionStorage = sessionStorage;
        _HttpClientService = httpClientService;

        BaseAddress = "http://localhost:7071";
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