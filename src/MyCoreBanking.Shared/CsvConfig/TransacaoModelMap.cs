using CsvHelper.Configuration;
using MyCoreBanking.Models;

namespace MyCoreBanking.CsvConfig;

public class TransacaoModelMap : ClassMap<TransacaoModel>
{
    public TransacaoModelMap()
    {
        Map(t => t.Id).Name("ID");
        Map(t => t.Descricao).Name("Descrição");
        Map(t => t.Observacao).Name("Observação");
        Map(t => t.Valor).Name("Valor");
        Map(t => t.Efetivada).Name("Efetivada");
        Map(t => t.DataEfetivacao).Name("Data da efetivação");
        Map(t => t.DataTransacao).Name("Data da transação/vencimento");
        Map(t => t.TipoOperacao).Name("Tipo de operação");
        Map(t => t.TipoTransacao).Name("Tipo de transação");
        Map(t => t.MeioPagamento).Name("Meio de pagamento");
        Map(t => t.Categoria).Name("Categoria");
        Map(t => t.Conta).Name("Conta");
        Map(t => t.ParcelaAtual).Name("Parcela atual");
        Map(t => t.NumeroParcelas).Name("Número de parcelas");
        Map(t => t.ReferenciaParcelaId).Name("ID do parcelamento");
    }
}