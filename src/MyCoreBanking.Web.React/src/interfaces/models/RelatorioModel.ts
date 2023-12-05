export default interface RelatorioModel {
  saldoTotal: number;
  totalInvestido: number;
  transacoesPendentes: number;
  balancoMensal: number;
  graficoMovimentacaoAnoAtual: any; // List<GraficoDespesaReceita>?
  graficoMovimentacaoUltimos12Meses: any; // List<GraficoDespesaReceita>?
  graficoDespesaPorCategoriaMensal: any; // List<GraficoDespesaPorCategoria>?
  graficoDespesaPorCategoriaAnual: any; // List<GraficoDespesaPorCategoria>?
}
