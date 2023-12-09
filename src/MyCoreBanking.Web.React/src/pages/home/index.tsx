import { FaBalanceScale, FaCalendarMinus, FaDollarSign } from "react-icons/fa";
import MainLayout from "../../components/layouts/MainLayout";
import { FaMoneyBillTrendUp } from "react-icons/fa6";
import { useEffect, useState } from "react";
import { getEstatisticas } from "../../services/estatisticas.service";
import RelatorioModel from "../../interfaces/models/RelatorioModel";
import toast from "react-hot-toast";
import { Spinner } from "flowbite-react";
import utils from "../../utils";
import LineChart from "./components/LineChart";

export default function Home() {
  const [loading, setLoading] = useState<boolean>(true);
  const [estatisticas, setEstatisticas] = useState<RelatorioModel>(
    {} as RelatorioModel
  );

  useEffect(() => {
    getEstatisticas()
      .then(({ data }: { data: RelatorioModel }) => {
        setEstatisticas(data);
      })
      .catch((error) => {
        toast.error(error.response.data.message);
      })
      .finally(() => setLoading(false));
  }, []);

  return (
    <MainLayout>
      <div className="flex flex-col items-center p-4">
        <span className="text-2xl font-bold">Bem-vindo ao MyCoreBanking</span>
        <span className="text-xl">
          Gerencie suas finanças de forma simples e fácil
        </span>
      </div>

      <div className="mt-4 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="flex justify-between items-center border-2 border-zinc-300 rounded-lg p-4 w-full">
          <div className="flex flex-col justify-center">
            <span className="text-xl font-bold text-blue-500">Seu saldo</span>
            <span className="text-md lg:text-lg">
              {loading ? (
                <Spinner />
              ) : (
                utils.formatCurrency(estatisticas.saldoTotal)
              )}
            </span>
          </div>
          <FaDollarSign className="text-4xl lg:text-5xl text-blue-500" />
        </div>

        <div className="flex justify-between items-center border-2 border-zinc-300 rounded-lg p-4 w-full">
          <div className="flex flex-col justify-center">
            <span className="text-xl font-bold text-green-400">
              Total investido
            </span>
            <span className="text-md lg:text-lg">
              {loading ? (
                <Spinner />
              ) : (
                utils.formatCurrency(estatisticas.totalInvestido)
              )}
            </span>
          </div>
          <FaMoneyBillTrendUp className="text-4xl lg:text-5xl text-green-400" />
        </div>

        <div className="flex justify-between items-center border-2 border-zinc-300 rounded-lg p-4 w-full">
          <div className="flex flex-col justify-center">
            <span className="text-xl font-bold text-yellow-400">
              Pendências
            </span>
            <span className="text-md lg:text-lg">
              {loading ? <Spinner /> : estatisticas.transacoesPendentes}
            </span>
          </div>
          <FaCalendarMinus className="text-4xl lg:text-5xl text-yellow-400" />
        </div>

        <div className="flex justify-between items-center border-2 border-zinc-300 rounded-lg p-4 w-full">
          <div className="flex flex-col justify-center">
            <span className="text-xl font-bold text-cyan-500">
              Balanço mensal
            </span>
            <span className="text-md lg:text-lg">
              {loading ? (
                <Spinner />
              ) : (
                utils.formatCurrency(estatisticas.balancoMensal)
              )}
            </span>
          </div>
          <FaBalanceScale className="text-4xl lg:text-5xl text-cyan-500" />
        </div>
      </div>

      <div className="mt-8">
        <span className="text-2xl font-bold">Estatísticas</span>
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
          <div className="sm:hidden">
            <span className="text-lg">
              Visualize as estatísticas dos gráficos de movimentações de entrada
              e saída em dispositivos maiores (tablets, notebooks, desktops).
            </span>
          </div>
          <div className="w-full lg:h-[700px] md:h-[500px] sm:h-[400px] hidden sm:block">
            {loading ? (
              <div className="flex justify-center items-center h-[400px]">
                <Spinner size="xl" />
              </div>
            ) : (
              <LineChart
                data={estatisticas.graficoMovimentacaoUltimos12Meses}
              />
            )}
          </div>
          <div className="w-full lg:h-[700px] md:h-[500px] sm:h-[400px] hidden sm:block">
            {loading ? (
              <div className="flex justify-center items-center h-[400px]">
                <Spinner size="xl" />
              </div>
            ) : (
              <LineChart
                data={estatisticas.graficoMovimentacaoUltimos12Meses}
              />
            )}
          </div>
        </div>
      </div>
    </MainLayout>
  );
}
