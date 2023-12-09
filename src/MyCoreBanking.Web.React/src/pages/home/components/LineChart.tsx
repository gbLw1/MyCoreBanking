import { Line } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";
import GraficoDespesaReceita from "../../../interfaces/models/GraficoDespesaReceita";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

const options = {
  responsive: true,
  plugins: {
    legend: {
      position: "top" as const,
    },
    title: {
      display: true,
      text: "Movimentações Entrada / Saída nos últimos 12 meses",
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
  data: GraficoDespesaReceita[] | null;
}

/**
 * Line chart component to display the last 12 months of income and expenses
 * @param data GraficoDespesaReceita[] | null;
 */
export default function LineChart({ data }: Props) {
  // get the current month and the 12 months before that
  // short month name / year
  const labels = Array.from(Array(13).keys())
    .map((i) => {
      const d = new Date();
      d.setMonth(d.getMonth() - i);
      return `${d.toLocaleString("default", {
        month: "short",
      })}/${d.getFullYear()}`;
    })
    .reverse() as string[];

  const chartData = {
    labels,
    datasets: [
      {
        label: "Receitas",
        data: data?.map((d) => d.valorReceita) || [],
        backgroundColor: "rgba(16, 161, 47, 0.95)",
        borderColor: "rgba(16, 161, 47, 0.95)",
      },
      {
        label: "Despesas",
        data: data?.map((d) => d.valorDespesa) || [],
        backgroundColor: "rgba(230, 40, 40, 0.95)",
        borderColor: "rgba(230, 40, 40, 0.95)",
      },
    ],
  };

  return <Line data={chartData} options={options} />;
}
