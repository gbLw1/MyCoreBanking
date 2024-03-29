﻿using MyCoreBanking.Args;
using MyCoreBanking.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MyCoreBanking.Web;

partial class MyCoreBankingApp
{
    public async Task<IReadOnlyCollection<TransacaoModel>?> ObterTransacoes(
        int mes,
        int ano,
        DateTime? dataEfetivacao = null,
        MeioPagamentoTipo? meioDePagamento = null,
        OperacaoTipo? tipoDeOperacao = null,
        TransacaoTipo? tipoDeTransacao = null,
        Categoria? categoria = null)
    {
        try
        {
            var sb = new StringBuilder();

            sb.Append($"{BaseAddress}/transacoes?mes={mes}&ano={ano}");

            if (dataEfetivacao.HasValue)
            {
                sb.Append($"&dataEfetivacao={dataEfetivacao.Value.ToString("yyyy-MM-dd")}");
            }

            if (meioDePagamento.HasValue)
            {
                sb.Append($"&meioDePagamento={meioDePagamento.Value.ToString()}");
            }

            if (tipoDeOperacao.HasValue)
            {
                sb.Append($"&tipoDeOperacao={tipoDeOperacao.Value.ToString()}");
            }

            if (tipoDeTransacao.HasValue)
            {
                sb.Append($"&tipoDeTransacao={tipoDeTransacao.Value.ToString()}");
            }

            if (categoria.HasValue)
            {
                sb.Append($"&categoria={categoria.Value.ToString()}");
            }

            var requestUri = sb.ToString();

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
                var result = JsonSerializer.Deserialize<IReadOnlyCollection<TransacaoModel>?>(responseContent)
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

    public async Task<TransacaoModel?> ObterTransacaoPorId(Guid transacaoId)
    {
        try
        {
            var requestUri = $"{BaseAddress}/transacoes/{transacaoId}";

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

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                _Navigation.NavigateTo("/transacoes");
                ShowError(responseContent);
                return null;
            }

            var result = JsonSerializer.Deserialize<TransacaoModel?>(responseContent)
                    ?? throw new InvalidOperationException(responseContent);

            return result;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return null;
        }
    }

    public async Task<IReadOnlyCollection<TransacaoModel>?> ObterParcelamentos(Guid parcelamentoId)
    {
        try
        {
            var requestUri = $"{BaseAddress}/transacoes?parcelamentoId={parcelamentoId}";

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

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                ShowError(responseContent);
                return null;
            }

            var result = JsonSerializer.Deserialize<IReadOnlyCollection<TransacaoModel>?>(responseContent)
                    ?? throw new InvalidOperationException(responseContent);

            return result;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return null;
        }
    }

    public async Task CadastrarTransacao(TransacoesPostArgs args)
    {
        try
        {
            var requestUri = $"{BaseAddress}/transacoes";

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

            _ToastService.ShowSuccess(message: "Transação cadastrada com sucesso!");
            _Navigation.NavigateTo("/transacoes");
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return;
        }
    }

    public async Task AlterarTransacao(Guid transacaoId, TransacoesPutArgs args, string? tipoUpdate = null)
    {
        try
        {
            var requestUri = $"{BaseAddress}/transacoes/{transacaoId}?tipoUpdate={tipoUpdate}";

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

            _ToastService.ShowSuccess(message:
                (tipoUpdate == "TODOS" || tipoUpdate == "PAGAMENTO-PENDENTE")
                ? "Transações alteradas com sucesso!"
                : "Transação alterada com sucesso!");
            _Navigation.NavigateTo("/transacoes");
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return;
        }
    }

    public async Task<bool> EfetivarTransacao(Guid transacaoId, TransacoesEfetivacaoPutArgs args)
    {
        try
        {
            var requestUri = $"{BaseAddress}/transacoes/{transacaoId}/efetivar";

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
                return false;
            }

            _ToastService.ShowSuccess(message: "Transação efetivada com sucesso!");
            return true;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return false;
        }
    }

    public async Task<bool> ExcluirTransacao(Guid transacaoId, string? tipoDelete = null)
    {
        try
        {
            // TEMP query string ↓
            var requestUri = $"{BaseAddress}/transacoes/{transacaoId}?tipoDelete={tipoDelete}";

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
                return false;
            }

            _ToastService.ShowSuccess(message: tipoDelete == "TODOS" ? "Transações excluídas com sucesso!" : "Transação excluída com sucesso!");
            return true;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return false;
        }
    }

}