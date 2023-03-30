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
}