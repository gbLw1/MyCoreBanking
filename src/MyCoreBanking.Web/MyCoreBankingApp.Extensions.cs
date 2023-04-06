namespace MyCoreBanking.Web;

partial class MyCoreBankingApp
{
    public static string ConverterEnumBancoParaString(Banco banco) => banco switch
    {
        Banco.BancoDoBrasil => "Banco do Brasil",
        Banco.Bradesco => "Bradesco",
        Banco.Inter => "Inter",
        Banco.Itau => "Itaú",
        Banco.Nubank => "Nubank",
        Banco.Santander => "Santander",
        Banco.C6 => "C6",
        Banco.Caixa => "Caixa",
        Banco.Outro => "Outro",
        _ => "Desconhecido",
    };

    public static string ConverterEnumTipoContaParaString(ContaTipo contaTipo) => contaTipo switch
    {
        ContaTipo.Corrente => "Corrente",
        ContaTipo.Poupanca => "Poupança",
        ContaTipo.Investimento => "Investimento",
        ContaTipo.Carteira => "Carteira pessoal",
        _ => "Desconhecido",
    };

    public static string ConverterEnumCategoriaParaString(Categoria categoria) => categoria switch
    {
        Categoria.Casa => "Casa",
        Categoria.Educacao => "Educação",
        Categoria.Eletronicos => "Eletrônicos",
        Categoria.Lazer => "Lazer",
        Categoria.Alimentacao => "Alimentação",
        Categoria.Transporte => "Transporte",
        Categoria.Saude => "Saúde",
        Categoria.Supermercado => "Supermercado",
        Categoria.Vestuario => "Vestuário",
        Categoria.Viagem => "Viagem",
        Categoria.Servico => "Serviço",
        Categoria.Investimentos => "Investimentos",
        Categoria.Presente => "Presente",
        Categoria.Salario => "Salário",
        Categoria.Outros => "Outros",
        _ => "Desconhecido",
    };

    public static string ConverterEnumTipoTransacaoParaString(TransacaoTipo tipo) => tipo switch
    {
        TransacaoTipo.Unica => "Única",
        TransacaoTipo.Parcelada => "Parcelada",
        _ => "Desconhecido",
    };

    public static string ConverterEnumMeioPagamentoParaString(MeioPagamentoTipo tipo) => tipo switch
    {

        MeioPagamentoTipo.Credito => "Crédito",
        MeioPagamentoTipo.Debito => "Débito",
        MeioPagamentoTipo.Pix => "PIX",
        MeioPagamentoTipo.Dinheiro => "Dinheiro",
        MeioPagamentoTipo.Cheque => "Cheque",
        MeioPagamentoTipo.Boleto => "Boleto",
        MeioPagamentoTipo.Transferencia => "Transferência",
        MeioPagamentoTipo.Outros => "Outros",
        _ => "Desconhecido",
    };
}