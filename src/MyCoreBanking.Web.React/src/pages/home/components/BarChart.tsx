import { Bar } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";
import GraficoDespesaPorCategoria from "../../../interfaces/models/GraficoDespesaPorCategoria";
import Categoria from "../../../interfaces/enums/Categoria";
import { graphColors } from "../../../constants/graph-colors";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

const options = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: "top" as const,
    },
    title: {
      display: true,
      text: `Total gasto em cada categoria neste ano (${new Date().getFullYear()})`,
    },
  },
  elements: {
    line: {
      tension: 0.4,
      borderWidth: 2,
    },
    point: {
      radius: 6,
    },
  },
  scales: {
    y: {
      beginAtZero: true,
    },
  },
};

interface Props {
  data: GraficoDespesaPorCategoria[] | null;
}

/**
 * Bar chart component to display the total spent in each category this year
 * @param data GraficoDespesaPorCategoria[] | null;
 */
export default function BarChart({ data }: Props) {
  const labels = Object.entries(Categoria).map(([key, value]) => value);

  const chartData = {
    labels,
    datasets: [
      {
        label: "Gastos",
        data: data?.map((d) => d.valor) || [],
        backgroundColor: graphColors,
        borderColor: graphColors,
      },
    ],
  };

  return <Bar data={chartData} options={options} />;
}
