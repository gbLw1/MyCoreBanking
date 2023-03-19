using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API;

public class CartoesPostExemplo
{
    public void ExemploCriarCartao()
    {
        var cartao = new CartaoDeCreditoEntity
        {
            NumerosFinais = "1234",
            Bandeira = BandeiraCartao.Mastercard,
            MeioDePagamento = new MeioDePagamentoEntity
            {
                Apelido = "Cartão de Crédito",
                Observacao = "Cartão de Crédito Mastercard",
                Tipo = MeioDePagamentoTipo.CartaoDeCredito,
                UsuarioId = Guid.Parse("<ALGUM_USUARIO_ID>"),
            },
        };
    }
}
