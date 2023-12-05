import { FaBalanceScale, FaCalendarMinus, FaDollarSign } from "react-icons/fa";
import MainLayout from "../../components/layouts/MainLayout";
import { FaMoneyBillTrendUp } from "react-icons/fa6";

export default function Home() {
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
            <span className="text-md lg:text-lg">R$ 1.000,00</span>
          </div>
          <FaDollarSign className="text-4xl lg:text-5xl text-blue-500" />
        </div>

        <div className="flex justify-between items-center border-2 border-zinc-300 rounded-lg p-4 w-full">
          <div className="flex flex-col justify-center">
            <span className="text-xl font-bold text-green-400">
              Total investido
            </span>
            <span className="text-md lg:text-lg">R$ 1.000,00</span>
          </div>
          <FaMoneyBillTrendUp className="text-4xl lg:text-5xl text-green-400" />
        </div>

        <div className="flex justify-between items-center border-2 border-zinc-300 rounded-lg p-4 w-full">
          <div className="flex flex-col justify-center">
            <span className="text-xl font-bold text-yellow-400">
              Pendências
            </span>
            <span className="text-md lg:text-lg">0</span>
          </div>
          <FaCalendarMinus className="text-4xl lg:text-5xl text-yellow-400" />
        </div>

        <div className="flex justify-between items-center border-2 border-zinc-300 rounded-lg p-4 w-full">
          <div className="flex flex-col justify-center">
            <span className="text-xl font-bold text-cyan-500">
              Balanço mensal
            </span>
            <span className="text-md lg:text-lg">R$ 1.000,00</span>
          </div>
          <FaBalanceScale className="text-4xl lg:text-5xl text-cyan-500" />
        </div>
      </div>
    </MainLayout>
  );
}
