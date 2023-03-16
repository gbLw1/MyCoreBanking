using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API;

public class CartoesPostExemplo
{
    public void ExemploCriarCartao()
    {
        var cartao = new MeioDePagamentoCartaoDeCredito
        {
            NumerosFinais = "1234",
            Bandeira = "Mastercard",
            MeioDePagamento = new MeioDePagamento
            {
                Apelido = "Cartão de Crédito",
                Observacao = "Cartão de Crédito Mastercard",
                Tipo = MeioDePagamentoTipo.CartaoDeCredito,
                UsuarioId = Guid.Parse("<ALGUM_USUARIO_ID>"),
            },
        };
    }
}
