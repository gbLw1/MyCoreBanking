using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using MyCoreBanking.Web.Shared.Components;

namespace MyCoreBanking.Web;

partial class MyCoreBankingApp
{
    /// <summary>
    /// <para>Permite exibir qualquer componente como um modal.</para>
    /// <para>Os parâmetros são passados como um dicionário de string e object. O componente deve ser preparado para receber os parâmetros.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parametros"></param>
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

    /// <summary>
    /// Exibe um modal de carregamento que impede o usuário de interagir com a aplicação.
    /// </summary>
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

    /// <summary>
    /// <para>Exibe um modal de erro com uma mensagem personalizada.</para>
    /// <para>A mensagem é exibida no corpo do modal com o título "Oops! Algo deu errado..."</para>
    /// </summary>
    /// <param name="message"></param>
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
}