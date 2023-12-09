import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";
import GraficoDespesaPorCategoria from "../../../interfaces/models/GraficoDespesaPorCategoria";
import { graphColors } from "../../../constants/graph-colors";

ChartJS.register(ArcElement, Tooltip, Legend);

const options = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: "top" as const,
    },
    title: {
      display: true,
      text: `Gastos por categoria neste mês (${new Date().toLocaleString(
        "default",
        {
          month: "long",
        }
      )})`,
    },
    tooltip: {
      backgroundColor: "white",
      titleColor: "black",
      bodyColor: "black",
    },
  },
};

interface Props {
  data: GraficoDespesaPorCategoria[] | null;
}

/**
 * Pie chart component to display the expenses by category in the current month
 * @param data GraficoDespesaPorCategoria[] | null;
 */
export default function PieChart({ data }: Props) {
  const labels = data?.map((d) => d.categoria) || [];

  const chartData = {
    labels,
    datasets: [
      {
        label: "Gastos por categoria neste mês",
        data: data?.map((d) => d.valor) || [],
        backgroundColor: graphColors,
        borderColor: graphColors,
        borderWidth: 1,
      },
    ],
  };

  return <Pie data={chartData} options={options} />;
}
