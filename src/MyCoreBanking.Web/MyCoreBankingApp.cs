using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using MyCoreBanking.Web.Shared.Components;

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

    #region [+ Modals]

    public IModalReference ShowModal<T>(
        IEnumerable<KeyValuePair<string, object>>? parametros = null)
        where T : IComponent
    {
        var options = new ModalOptions()
        {
            OverlayCustomClass = "no-overlay",
            HideHeader = true,
            HideCloseButton = true,
            AnimationType = ModalAnimationType.None,
            Size = ModalSize.Medium,
            Position = ModalPosition.Middle,
        };

        if (parametros is not null)
        {
            var parameters = new ModalParameters();

            foreach (var p in parametros)
            {
                parameters.Add(p.Key, p.Value);
            }

            return _ModalService.Show<T>(string.Empty, parameters, options);
        }
        else
        {
            return _ModalService.Show<T>(string.Empty, options);
        }
    }

    public IModalReference ShowLoading()
    {
        var options = new ModalOptions()
        {
            HideHeader = true,
            HideCloseButton = true,
            DisableBackgroundCancel = true,
            AnimationType = ModalAnimationType.None,
            Size = ModalSize.Small,
            Position = ModalPosition.Middle,
            Class = "modal-full-screen",
        };

        return _ModalService.Show<LoadingModal>(string.Empty, options);
    }

    public IModalReference ShowError(string message)
    {
        var parameters = new ModalParameters()
            .Add(nameof(ErrorModal.Message), message);

        var options = new ModalOptions()
        {
            //HideHeader = true,
            //HideCloseButton = true,
            AnimationType = ModalAnimationType.None,
            Size = ModalSize.Medium,
            Position = ModalPosition.TopCenter,
        };

        return _ModalService.Show<ErrorModal>("Oops! Algo deu errado...", parameters, options);
    }

    #endregion

}