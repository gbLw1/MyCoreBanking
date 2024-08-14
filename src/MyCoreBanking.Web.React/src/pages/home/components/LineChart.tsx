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
  maintainAspectRatio: false,
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
 * Line chart component to display the current and last 12 months of income and expenses
 * @param data GraficoDespesaReceita[] | null;
 */
export default function LineChart({ data }: Props) {
  // get the current month and the 12 months before that
  // short month name / year
  const labels = Array.from(Array(13).keys())
    .map((i) => {
      const d = new Date();
      d.setMonth(d.getMonth() - i);
      return `${d.toLocaleString("default", { month: "short" })}/${d.getFullYear()}`;
    })
    .reverse() as string[];

  // ===================================================================

  // add 3 items to `data`
  data = [
    { valorReceita: 0, valorDespesa: 0, ano: 2024, mes: 6 },
    { valorReceita: 0, valorDespesa: 0, ano: 2024, mes: 6 },
    ...(data || []),
  ];

  // ex: API retorna 04/06/2024, 05/06/2024, 06/06/2024, 07/06/2024...
  // dos últimos 15 dias (por padrão)
  data =
    data?.map((d, i) => {
      const date = new Date();
      date.setDate(date.getDate() - i);
      return { ...d, date };
    }) || [];

  console.log(
    data.map((g, i) => {
      return {
        date: g.date?.toISOString(),
        status: "emitido",
        quantidade: 1 + i,
      };
    })
  );

  const day_month_label: string[] =
    data
      ?.map((g) => {
        const date: Date = new Date(g.date!);

        // return format: 15/Fev, 16/Fev, 18/Fev, 19/Fev...
        return `${date.getDate()}/${date.toLocaleString("default", { month: "short" })}`;
      })
      .reverse() || [];

  // ===================================================================

  const chartData = {
    labels: day_month_label,
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
