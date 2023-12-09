import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";
import GraficoDespesaPorCategoria from "../../../interfaces/models/GraficoDespesaPorCategoria";

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
        backgroundColor: [
          "#FF6384",
          "#36A2EB",
          "#FFCE56",
          "#33FF99",
          "#FF5733",
          "#8A2BE2",
          "#7FFFD4",
          "#B0E0E6",
          "#FFD700",
          "#7FFF00",
          "#CD5C5C",
          "#9370DB",
          "#00FF7F",
          "#20B2AA",
        ],
        borderColor: [
          "lavender",
          "gray",
          "aqua",
          "blue",
          "darkblue",
          "peru",
          "brown",
          "magenta",
          "darkviolet",
          "hotpink",
          "yellow",
          "orange",
          "red",
          "lightgreen",
          "green",
        ],
        borderWidth: 1,
      },
    ],
  };

  return <Pie data={chartData} options={options} />;
}
