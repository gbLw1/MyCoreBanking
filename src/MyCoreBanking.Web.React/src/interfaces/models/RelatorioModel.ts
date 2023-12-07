import GraficoDespesaPorCategoria from "./GraficoDespesaPorCategoria";
import GraficoDespesaReceita from "./GraficoDespesaReceita";

export default interface RelatorioModel {
  saldoTotal: number;
  totalInvestido: number;
  transacoesPendentes: number;
  balancoMensal: number;
  graficoMovimentacaoAnoAtual: GraficoDespesaReceita[] | null;
  graficoMovimentacaoUltimos12Meses: GraficoDespesaReceita[] | null;
  graficoDespesaPorCategoriaMensal: GraficoDespesaPorCategoria[] | null;
  graficoDespesaPorCategoriaAnual: GraficoDespesaPorCategoria[] | null;
}
